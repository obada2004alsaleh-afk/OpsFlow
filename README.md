# OpsFlow

OpsFlow is a Service Operations and SLA Management Platform designed to help service and maintenance teams manage customer requests, technician assignments, ticket workflows, and service-level agreements in one centralized system.

## Problem

Service operations are often managed through scattered communication channels such as phone calls and messaging applications.

This can lead to:

- Delayed responses
- Unclear responsibilities
- Lost or fragmented information
- Difficulty tracking ticket progress
- Difficulty measuring service performance

OpsFlow aims to centralize these operations into a structured, traceable, and maintainable system.

## Technology Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Clean Architecture
- Modular Monolith
### Frontend

- React
- JavaScript

### Development Tooling

- Vite
- ESLint
- Git
- GitHub

### Database

- SQL Server
- Entity Framework Core planned for the implementation layer


## Project Structure

```text
OpsFlow/
├── backend/
│   ├── OpsFlow.Api/
│   ├── OpsFlow.Application/
│   ├── OpsFlow.Domain/
│   ├── OpsFlow.Infrastructure/
│   └── OpsFlow.slnx
│
├── frontend/
│   └── opsflow-web/
│
├── .gitignore
└── README.md
```

## Architecture

The backend follows Clean Architecture using a modular monolith approach.

The current project dependency direction is:

```text
Application → Domain

Infrastructure → Application
Infrastructure → Domain

API → Application
API → Infrastructure
```

The goal is to keep business rules independent from technical concerns such as database access and external infrastructure.

### Responsibilities

**Domain**

Contains the core business entities and business rules.

**Application**

Coordinates application use cases and depends on the Domain layer.

**Infrastructure**

Provides technical implementations such as database access and other infrastructure services.

**API**

Acts as the backend entry point and composition layer for the application.

**Frontend**

A separate React application responsible for the user interface.

## Running the Backend

From the project root, build the backend solution:

```powershell
dotnet build .\backend\OpsFlow.slnx
```

Run the API:

```powershell
dotnet run --project .\backend\OpsFlow.Api\OpsFlow.Api.csproj
```

## Running the Frontend

Move to the frontend project:

```powershell
cd .\frontend\opsflow-web
```

Install dependencies when setting up the project for the first time:

```powershell
npm install
```

Start the development server:

```powershell
npm run dev
```

Vite will provide the local development URL in the terminal.

## Planned V1 Capabilities

The first version of OpsFlow is planned to include:

- Authentication
- Role-based authorization
- User management
- Customers, sites, and assets
- Ticket creation and tracking
- Ticket assignment
- Technician workflow
- Repair logs and work notes
- Ticket status history
- Priority management
- SLA response and resolution tracking
- Search, filtering, and pagination
- Basic dashboard
- Validation and error handling
- Logging
- Automated tests
- API documentation
- CI/CD
- Deployment

## Current Status

**Status: In Development**

The project foundation has been established.

Completed so far:

- Product definition
- Business workflow definition
- Database design and ERD
- Architecture design
- Backend solution structure
- Clean Architecture project dependencies
- React frontend foundation
- Git repository setup
- GitHub repository integration

Feature implementation is currently in progress.