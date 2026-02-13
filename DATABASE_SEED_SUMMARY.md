# Database Seed Data Summary

## Successfully Seeded Data

### ?? **Users (6 Total)**

#### **Admin (1)**
- **Email**: admin@lms.com
- **Password**: Admin@123
- **Role**: Admin

#### **Instructors (3)**
1. **John Smith**
   - Email: john.smith@lms.com
   - Password: Instructor@123
   - Courses: React, ASP.NET Core, Azure, Digital Marketing

2. **Sarah Johnson**
   - Email: sarah.johnson@lms.com
   - Password: Instructor@123
   - Courses: Full-Stack JavaScript, iOS Development, AWS, UI/UX Design

3. **Michael Chen**
   - Email: michael.chen@lms.com
   - Password: Instructor@123
   - Courses: Flutter, Python Data Science, Deep Learning, Docker/Kubernetes, Git/GitHub, Python Intro

#### **Students (2)**
1. **Emma Davis**
   - Email: emma.davis@student.com
   - Password: Student@123
   - Enrolled: React Developer (45.5% progress), Full-Stack JavaScript (20% progress)

2. **James Wilson**
   - Email: james.wilson@student.com
   - Password: Student@123
   - Enrolled: React Developer (60% progress), Python Intro (100% completed)

---

### ?? **Categories (7)**
1. **Web Development** - Learn web technologies and frameworks
2. **Mobile Development** - Build mobile applications
3. **Data Science** - Master data analysis and machine learning
4. **Cloud Computing** - Learn cloud platforms and services
5. **DevOps** - CI/CD and infrastructure automation
6. **Design** - UI/UX and graphic design
7. **Business** - Management and entrepreneurship

---

### ?? **Courses (14 Total)**

#### **Web Development (3 courses)**
1. **Complete React Developer Course** - $89.99
   - Instructor: John Smith
   - Level: Intermediate | Duration: 40 hours
   - Image: React development workspace
   - 3 Sections, 3 Lessons seeded

2. **ASP.NET Core Web API Development** - $99.99
   - Instructor: John Smith
 - Level: Advanced | Duration: 35 hours
   - Image: Coding on laptop

3. **Full-Stack JavaScript Bootcamp** - $129.99
- Instructor: Sarah Johnson
   - Level: Beginner | Duration: 60 hours
   - Image: JavaScript code

#### **Mobile Development (2 courses)**
4. **iOS App Development with Swift** - $94.99
   - Instructor: Sarah Johnson
   - Level: Intermediate | Duration: 45 hours
   - Image: iPhone development

5. **Flutter & Dart - Complete Guide** - $84.99
   - Instructor: Michael Chen
   - Level: Beginner | Duration: 50 hours
   - Image: Mobile app development

#### **Data Science (2 courses)**
6. **Python for Data Science and Machine Learning** - $119.99
   - Instructor: Michael Chen
   - Level: Intermediate | Duration: 55 hours
   - Image: Data visualization charts

7. **Deep Learning with TensorFlow** - $139.99
   - Instructor: John Smith
   - Level: Advanced | Duration: 48 hours
   - Image: AI/Neural networks

#### **Cloud Computing (2 courses)**
8. **AWS Certified Solutions Architect** - $109.99
   - Instructor: Sarah Johnson
   - Level: Intermediate | Duration: 42 hours
   - Image: Cloud computing concept

9. **Microsoft Azure Fundamentals** - $69.99
   - Instructor: John Smith
   - Level: Beginner | Duration: 30 hours
   - Image: Azure services

#### **DevOps (1 course)**
10. **Docker and Kubernetes Mastery** - $99.99
    - Instructor: Michael Chen
    - Level: Advanced | Duration: 38 hours
    - Image: Container orchestration

#### **Design (1 course)**
11. **UI/UX Design Fundamentals** - $59.99
    - Instructor: Sarah Johnson
    - Level: Beginner | Duration: 28 hours
    - Image: Design workspace

#### **Business (1 course)**
12. **Digital Marketing Masterclass** - $74.99
    - Instructor: John Smith
    - Level: Beginner | Duration: 32 hours
    - Image: Marketing analytics

#### **Free Courses (2)**
13. **Introduction to Programming with Python** - FREE
    - Instructor: Michael Chen
    - Level: Beginner | Duration: 20 hours
    - Image: Python programming

14. **Git and GitHub Essentials** - FREE
 - Instructor: Sarah Johnson
    - Level: Beginner | Duration: 15 hours
    - Image: Version control

---

### ?? **Course Content (Sample)**

**React Developer Course** includes:
- **Section 1**: React Fundamentals
  - Lesson 1: Introduction to React (15 min) - FREE
  - Lesson 2: Components and Props (20 min)
  - Lesson 3: State and Lifecycle (25 min)
- **Section 2**: Advanced React Patterns
- **Section 3**: State Management with Redux

---

### ?? **Enrollments (4)**
1. Emma Davis ? React Developer (45.5% progress)
2. Emma Davis ? Full-Stack JavaScript (20% progress)
3. James Wilson ? React Developer (60% progress)
4. James Wilson ? Python Intro (100% completed) ?

---

### ?? **Reviews (3)**
1. **James Wilson** on React Developer: ????? (5/5)
   - "Excellent course! Very well structured and easy to follow."

2. **Emma Davis** on Full-Stack JavaScript: ???? (4/5)
   - "Great content, but could use more practical examples."

3. **James Wilson** on Python Intro: ????? (5/5)
   - "Perfect for beginners! Highly recommended."

---

### ?? **Progress Tracking**
- Emma Davis: Completed 2 lessons in React course
- James Wilson: Completed 1 lesson in React course

---

## ?? **Image Sources**

All course thumbnails use high-quality images from **Unsplash**:
- Professional, royalty-free stock photos
- Relevant to course topics
- Optimized for web display (800px width, quality 80)

---

## ?? **Testing Credentials**

### **For Testing Admin Features:**
- Email: `admin@lms.com`
- Password: `Admin@123`

### **For Testing Instructor Features:**
- Email: `john.smith@lms.com`
- Password: `Instructor@123`

### **For Testing Student Features:**
- Email: `emma.davis@student.com`
- Password: `Student@123`

---

## ?? **Migration Applied**

**Migration Name**: `20260212141139_SeedInitialData`

**Command Used**:
```bash
dotnet ef database update --project LearningManagementSystem.Infrastructrue\LearningManagementSystem.Infrastructrue.csproj --startup-project LearningManagementSystem.API\LearningManagementSystem.API.csproj
```

---

## ? **Verification**

Database successfully populated with:
- ? 6 Users (1 Admin, 3 Instructors, 2 Students)
- ? 7 Categories
- ? 14 Courses (12 paid + 2 free)
- ? 3 Sections (React course)
- ? 3 Lessons (React fundamentals)
- ? 4 Enrollments
- ? 3 Progress records
- ? 3 Reviews

All data is ready for testing the complete LMS application! ??
