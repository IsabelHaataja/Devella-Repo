# Devella

Devella is a full-stack web application built with ASP.NET Core (.NET 9), Blazor Web, and MySQL, deployed to Oracle Cloud using Docker and GitHub Actions CI/CD.

The project was originally created as a higher vocational thesis in the .NET Developer program and focused on building a functional MVP of the idea, using modern .NET technologies. I've since then deployed it, continued developing, adding features and making improvements.

https://devella-demo.se/
## Purpose

The idea of Devella is to bridge the gap between education and the job market by:
- Allowing developers early in their careers to showcase their skills and experience
- Helping startups and small companies find suitable developers for MVPs, prototypes, internships, or early projects


## Key Features

- User registration and authentication (developer or company roles)
- Developer profile creation and editing
- Search functionality for companies to find developers by skills and experience
- Role-based access using JWT authentication
- REST API for all data and business logic

## Tech Stack

- **Frontend:** Blazor Web
- **Backend:** ASP.NET Core Web API  
- **Authentication:** ASP.NET Identity + JWT  
- **Database:** MySQL 8 + Entity Framework Core (pomelo)
- **Testing:** xUnit (unit tests)
- **CI/CD:** Github Actions
- **Hosting:** Oracle Cloud (Free Tier ARM VM)
- **Reverse Proxy & HTTPS:** Caddy (Let's encrypt)
- **Conteinerization:** Docker & Docker Compose

## Architecture Overview

The solution is divided into multiple projects with clear separation of concerns:
- Web API for data access and authentication
- Blazor frontend consuming the API
- Shared class library for domain models, DTOs, mappers, enums and services

This structure improves maintainability, testability, and scalability.

## Environments 
| Environment | Description |
|------------|-------------|
| Local | Docker Compose (MySQL only) |
| Production | Oracle Cloud VM (Docker Compose) |

## Running locally

### Prerequisites
- .NET 9 SDK
- Docker Desktop

### Start MySQL locally
- bash
docker compose -f docker-compose.mysql.yml up -d

### Run the solution
dotnet run --project Devella.API
dotnet run --project Devella

## Deployment
The application is deployed on an Oracle Cloud ARM VM using Docker.

1. Push to `main`
2. GitHub Actions runs build & tests
3. GitHub Actions connects to the VM via SSH
4. VM pulls latest code
5. Containers are rebuilt and restarted

### CI/CD
Two GitHub Actions workflows are used:

- **ci.yml**
  - Runs on pull requests and pushes
  - Restores, builds, and tests the solution

- **deploy.yml**
  - Runs on pushes to `main`
  - Deploys the application to the Oracle VM

## Visit live website
[Go to Devella](https://devella-demo.se/)
