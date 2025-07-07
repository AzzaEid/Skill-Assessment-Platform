# 🎓 Skill Assessment Platform – Graduation Project from PTUK
## Backend

A backend system for evaluating technical skills through structured **tracks**, **levels**, and **stages**, supporting multiple assessment types like **exams, interviews, and tasks**. The platform manages the complete workflow from applicant enrollment to certification, including workload distribution for examiners and detailed progress tracking.

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/AzzaEid/Skill-Assessment-Platform)

---

## 🎯 Purpose & Scope

Companies waste time and money filtering applicants due to a gap between CVs and real skills. Our platform aims to **bridge this gap** by enabling multi-method skill assessments with dynamic tracks and levels, plus customizable evaluation criteria that adapt to market needs.

---

## 🏛️ Architecture

Built on **Clean Architecture** with clear separation into four layers:

* **API Layer**: Controllers, authentication middleware, centralized error handling
* **Application Layer**: Business logic, services, DTO mapping, validation
* **Core Layer**: Domain models, interfaces, core business rules
* **Infrastructure Layer**: Data access (EF Core), repositories, Unit of Work, external integrations (email, Zoom)

<img width="817" height="858" alt="Image" src="https://github.com/user-attachments/assets/e8ff5bae-fb82-488f-9413-0dd41eb7d678" />

Key components:

* **TracksController**, **AuthController**, **ErrorHandlerMiddleware**, **ResponseHandler**
* **Services**: AuthService, TrackService, StageProgressService, ExaminerLoadsService
* **Entities & Interfaces**: Track, StageProgress, IUnitOfWork, ITrackRepository
* **Infrastructure**: TrackRepository, UnitOfWork, AppDbContext, EmailService, CacheService

---

## 🗂️ Dependency Injection

Modular DI setup ensures clean registration of each layer’s services:

* `AddApplicationDependencies()`: Registers core services (AuthService, TrackService, etc.)
* `AddInfrastructureDependencies()`: Registers repositories, UnitOfWork, external services (Email, Zoom)
* `AddServiceRegistration()`: Configures Identity, JWT auth, Swagger

---

## 🛠️ Data Model

Hierarchical structure with:

* **Tracks → Levels → Stages**
* Progress tracking: Enrollments → LevelProgress → StageProgress
* Assessment types: Exams, Interviews, Task Submissions
* Examiner workload: ExaminerLoads, CreationAssignments
* Feedback and certificates

---

## 💻 Technology Stack

| Component            | Technology             |
| -------------------- | ---------------------- |
| Runtime              | .NET 8.0               |
| Web Framework        | ASP.NET Core           |
| Database             | SQL Server + EF Core   |
| Authentication       | ASP.NET Core Identity  |
| Authorization        | JWT Bearer Tokens      |
| Object Mapping       | AutoMapper             |
| API Docs             | Swagger/OpenAPI        |
| Notifications        | NETCore.MailKit (SMTP) |
| External Integration | Zoom API               |

---

## 🔗 Data Access

Implements **Repository** and **Unit of Work Patterns**:

* Provides clean, testable, and transactional data operations.
* Reduces code duplication with a **Generic Repository**.
* Repositories cover user management, assessments, progress tracking, workload, evaluation, and more.

---

## 🔐 Authentication & Security

* **ASP.NET Identity** for secure user management with seeded roles.
* **JWT Bearer Authentication** for protecting APIs.
* **CORS policy** configured for safe multi-client integration.
* Password reset and email confirmation tokens.
* Centralized authentication flow using AuthService and Identity UserManager.

---

## 📌 Features Highlights

✅ Dynamic technical tracks with customizable evaluation criteria
✅ Support for multiple assessment types (exams, interviews, tasks)
✅ Examiner workload distribution for fair assignment
✅ FluentValidation for DTO validation
✅ Exception handling middleware
✅ Hybrid caching for performance optimization
✅ Email notifications (SMTP) for applicant updates
✅ Zoom API integration for scheduling interviews
✅ Interactive Swagger documentation for easy API exploration

---

## 📂 Attachments

* Project cover page
* Architecture diagram
* ERD diagram
* Swagger API screenshots

---

### 🚀 Ready to explore? Clone the repo and dive in!

