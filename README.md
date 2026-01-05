# Devella

Devella is a web-based platform that connects IT students and junior developers with small and early-stage companies in need of technical talent.  
The project was developed as a higher vocational thesis in the .NET Developer program and focuses on building a functional MVP using modern .NET technologies.

## 🎯 Purpose

The goal of Devella is to bridge the gap between education and the job market by:
- Allowing developers early in their careers to showcase their skills and experience
- Helping startups and small companies find suitable developers for MVPs, internships, or early projects

The project emphasizes **functionality, architecture, and backend/frontend integration** rather than visual design.

## 🧩 Key Features

- User registration and authentication (developer or company roles)
- Developer profile creation and editing
- Search functionality for companies to find developers by skills and experience
- Role-based access using JWT authentication
- REST API for all data and business logic

## 🛠 Tech Stack

- **Backend:** ASP.NET Core Web API  
- **Frontend:** Blazor Server  
- **Authentication:** ASP.NET Identity + JWT  
- **Database:** SQL Server + Entity Framework Core  
- **Architecture:** Layered architecture inspired by Clean Architecture  
- **Testing:** xUnit (unit tests)

## 🏗 Architecture Overview

The solution is divided into multiple projects with clear separation of concerns:
- Web API for data access and authentication
- Blazor frontend consuming the API
- Shared class libraries for domain models, DTOs, and services

This structure improves maintainability, testability, and scalability.

## 🚧 Scope & Limitations

This project was developed within a four-week timeframe and is intended as a **technical MVP**.  
Features such as automated matching, notifications, and third-party integrations were intentionally excluded to keep the focus on core system design and implementation.

## 📚 What This Project Demonstrates

- Full-stack development with .NET and Blazor
- Secure authentication and authorization
- REST API design
- Database design and ORM usage
- Clean, structured architecture suitable for real-world systems

## 📄 Thesis Report

The full thesis report (in Swedish) is included in this repository and provides detailed background, technical decisions, and reflections on the project. 
(https://github.com/user-attachments/files/24401712/Examensarbete_v2.pdf)
---

