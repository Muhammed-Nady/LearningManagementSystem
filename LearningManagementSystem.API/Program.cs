using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Infrastructrue.Data;
using LearningManagementSystem.Infrastructrue.Repositories;
using LearningManagementSystem.Infrastructrue.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LearningManagementSystem.API
{
    public class Program
    {
     public static void Main(string[] args)
        {
         var builder = WebApplication.CreateBuilder(args);

            // Database Configuration
            ConfigureDatabase(builder);

          // Dependency Injection
       ConfigureRepositories(builder);
      ConfigureServices(builder);

    // Authentication & Authorization
     ConfigureAuthentication(builder);
            ConfigureAuthorization(builder);

            // CORS Configuration
     ConfigureCors(builder);

    // API Controllers
            builder.Services.AddControllers();

            // Swagger Configuration
         ConfigureSwagger(builder);

            var app = builder.Build();

        // Configure HTTP Request Pipeline
  ConfigurePipeline(app);

    app.Run();
        }

      private static void ConfigureDatabase(WebApplicationBuilder builder)
      {
            builder.Services.AddDbContext<ApplicationDbContext>(
 options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }

        private static void ConfigureRepositories(WebApplicationBuilder builder)
   {
  builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
 }

   private static void ConfigureServices(WebApplicationBuilder builder)
 {
     builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
       builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
         builder.Services.AddScoped<IProgressService, ProgressService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
       builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IUserService, UserService>();
      }

        private static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");

  builder.Services.AddAuthentication(options =>
            {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
 {
            options.TokenValidationParameters = new TokenValidationParameters
{
           ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
              ValidateIssuer = true,
     ValidIssuer = jwtSettings["Issuer"],
      ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
         ValidateLifetime = true,
  ClockSkew = TimeSpan.Zero
     };
            });
        }

        private static void ConfigureAuthorization(WebApplicationBuilder builder)
        {
   builder.Services.AddAuthorization();
        }

        private static void ConfigureCors(WebApplicationBuilder builder)
        {
    var corsSettings = builder.Configuration.GetSection("Cors");
            builder.Services.AddCors(options =>
   {
        options.AddPolicy(corsSettings["PolicyName"] ?? "AllowMvcClient", policy =>
 {
  var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
             policy.WithOrigins(allowedOrigins)
      .AllowAnyMethod()
       .AllowAnyHeader()
  .AllowCredentials();
     });
      });
        }

      private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
  builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
    {
              c.SwaggerDoc("v1", new OpenApiInfo
             {
     Title = "Learning Management System API",
       Version = "v1",
     Description = "API for managing courses, enrollments, and student progress"
                });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
             Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
      Name = "Authorization",
             In = ParameterLocation.Header,
     Type = SecuritySchemeType.ApiKey,
           Scheme = "Bearer"
     });

           c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
     new OpenApiSecurityScheme
    {
         Reference = new OpenApiReference
        {
  Type = ReferenceType.SecurityScheme,
    Id = "Bearer"
         }
   },
        Array.Empty<string>()
      }
              });
            });
      }

        private static void ConfigurePipeline(WebApplication app)
        {
  if (app.Environment.IsDevelopment())
      {
     app.UseSwagger();
  app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

          var corsSettings = app.Configuration.GetSection("Cors");
        app.UseCors(corsSettings["PolicyName"] ?? "AllowMvcClient");

            app.UseAuthentication();
    app.UseAuthorization();

            app.MapControllers();
        }
    }
}
