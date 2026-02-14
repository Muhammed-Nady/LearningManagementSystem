# Learning Management System

## Overview

The Learning Management System is a modular and scalable web application built using ASP.NET Core. It follows clean architecture principles to ensure clear separation of concerns, maintainability, and extensibility. The system demonstrates how to design and implement a real-world platform that manages courses, users, and enrollments through a structured backend API and an MVC-based frontend.

This project is intended as a practical example of building production-style software with a focus on backend engineering practices and system design.

---

## Features

### User Management

* User registration and authentication
* Role-based access control (such as Administrator, Instructor, and Student)

### Course Management

* Create, update, and manage courses
* Organize course information and content

### Enrollment Management

* Allow students to enroll in courses
* Track enrollment records

### Administrative Functions

* Manage users and courses
* Monitor system operations

### API Integration

* RESTful API that exposes application functionality
* MVC application consuming API endpoints

---

## Architecture

The solution is structured into multiple layers to enforce separation of responsibilities and improve maintainability.

LearningManagementSystem

* LearningManagementSystem.API — Handles HTTP requests and exposes REST endpoints
* LearningManagementSystem.MVC — Presentation layer responsible for user interface
* LearningManagementSystem.Core — Domain entities, interfaces, and business rules
* LearningManagementSystem.Infrastructure — Data access, database context, and external integrations

Dependency flow is designed so that higher-level layers depend on abstractions defined in the core domain, while infrastructure implements those abstractions.

---

## Technology Stack

* ASP.NET Core
* C#
* ASP.NET Core MVC
* Web API
* Entity Framework Core
* SQL Server
* Dependency Injection
* Razor Views
* HTTP Client

---

## Getting Started

### Clone the repository

git clone https://github.com/Muhammed-Nady/LearningManagementSystem.git

cd LearningManagementSystem

### Configure the database

Update the connection string in the appsettings.json file to match your local database configuration.

### Apply database migrations

dotnet ef database update

### Run the application

Start both the API and MVC projects, then navigate to the MVC application in your browser.

---

## Project Structure

Core
Contains domain entities, interfaces, and business logic.

Infrastructure
Contains database context, repository implementations, and external service integrations.

API
Contains controllers and endpoint definitions.

MVC
Contains views, controllers, and client-side interactions.

---

## Purpose

The primary goal of this project is to demonstrate the design and implementation of a layered application that reflects real-world software development practices. It serves as a learning platform for understanding backend architecture and as a portfolio project that showcases engineering skills.

---

## Author

Mohammed Nady
Computer Science and Artificial Intelligence student with a focus on backend development and scalable system design.
