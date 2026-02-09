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

            // Add services to the container.
            
            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository Pattern & Unit of Work
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IProgressService, ProgressService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // JWT Authentication
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

            builder.Services.AddAuthorization();

            // CORS
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

            builder.Services.AddControllers();

            // Swagger with JWT support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
  c.SwaggerDoc("v1", new OpenApiInfo 
                { 
          Title = "Learning Management System API", 
      Version = "v1",
   Description = "API for managing courses, enrollments, and student progress"
          });

        // Add JWT Authentication to Swagger
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

       var app = builder.Build();

            // Configure the HTTP request pipeline.
     if (app.Environment.IsDevelopment())
  {
        app.UseSwagger();
  app.UseSwaggerUI();
 }

            app.UseHttpsRedirection();

     // Use CORS
   app.UseCors(corsSettings["PolicyName"] ?? "AllowMvcClient");

         // Authentication & Authorization
  app.UseAuthentication();
      app.UseAuthorization();

  app.MapControllers();

         app.Run();
   }
    }
}
