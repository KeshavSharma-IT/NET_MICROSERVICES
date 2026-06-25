# 🚀 Cloud-Native E-Commerce Microservices Platform

A cloud-native e-commerce backend built with **ASP.NET Core Microservices**, following **Clean Architecture** and modern backend development practices. The solution demonstrates service decomposition, containerization, orchestration, asynchronous messaging, caching, and CI/CD using Azure DevOps.

---

# 📌 Overview

This project was built to learn and implement enterprise-level backend architecture using the Microsoft technology stack.

The application is divided into independent microservices that communicate through REST APIs and asynchronous messaging, making the system scalable, maintainable, and independently deployable.

---

# 🏗️ Architecture

```
                        Client / Frontend
                               │
                               ▼
                         API Gateway
                               │
        ┌──────────────┬──────────────┬──────────────┐
        ▼              ▼              ▼
  User Service    Product Service   Order Service
        │              │              │
        ▼              ▼              ▼
 PostgreSQL        MySQL          MongoDB

                ▲
                │
            RabbitMQ
                │
            Redis Cache
```

---

# ✨ Features

* Microservices-based architecture
* RESTful APIs
* API Gateway
* User Authentication
* Product Management
* Order Management
* Entity Framework Core
* Dapper
* Repository Pattern
* Dependency Injection
* Clean Architecture
* Docker & Docker Compose
* Kubernetes (AKS)
* Redis Caching
* RabbitMQ Messaging
* Azure DevOps CI/CD
* Exception Handling Middleware
* Validation
* Unit Testing

---

# 🛠️ Technology Stack

## Backend

* ASP.NET Core
* C#
* REST APIs
* Entity Framework Core
* Dapper
* AutoMapper
* FluentValidation

## Databases

* PostgreSQL
* MySQL
* MongoDB

## DevOps

* Docker
* Docker Compose
* Kubernetes (AKS)
* Azure DevOps Pipelines

## Messaging & Caching

* RabbitMQ
* Redis

---

# 📂 Project Structure

```
NET_MICROSERVICES

├── User Service
│   ├── API
│   ├── Core
│   ├── Infrastructure
│   └── Unit Tests
│
├── Product Service
│   ├── API
│   ├── Core
│   ├── Infrastructure
│   └── Unit Tests
│
├── Order Service
│   ├── API
│   ├── Core
│   ├── Infrastructure
│   └── Unit Tests
│
├── API Gateway
├── Docker
├── Kubernetes (AKS)
├── Redis
├── PostgreSQL
├── MongoDB
├── MySQL
└── Frontend
```

---

# 🏛️ Architecture Pattern

Each microservice follows **Clean Architecture**.

```
API
   │
Core
   │
Infrastructure
```

Core contains:

* Business Logic
* Interfaces
* DTOs
* Entities

Infrastructure contains:

* Entity Framework Core
* Dapper
* Repository Implementations
* Database Context
* External Services

API contains:

* Controllers
* Middleware
* Dependency Injection
* Authentication
* Configuration

---

# 🐳 Running with Docker

```bash
docker compose up --build
```

---

# ☸️ Deploying to Kubernetes

```bash
kubectl apply -f aks/
```

---

# 🔄 CI/CD

The project includes Azure DevOps pipeline configuration to automate:

* Build
* Test
* Docker Image Creation
* Deployment

---

# 📚 Learning Objectives

This project was built to gain hands-on experience with:

# ✨ Features

- Microservices Architecture
- RESTful APIs
- API Gateway
- JWT Authentication & Refresh Tokens
- Role-Based Authorization
- API Rate Limiting
- Entity Framework Core
- Dapper
- Repository Pattern
- Dependency Injection
- Clean Architecture
- Docker & Docker Compose
- Kubernetes (AKS)
- RabbitMQ Messaging
- Redis Caching
- Serilog Structured Logging
- Polly Resilience Policies
- Azure DevOps CI/CD
- Exception Handling Middleware
- Global Error Handling
- Validation
- Unit Testing

---

# 🚀 Future Improvements


* OpenTelemetry
* Health Checks
* Distributed Tracing
* Event Sourcing
* Saga Pattern
* Centralized Configuration

---

# 👨‍💻 Author

**Keshav Sharma**

* .NET Full Stack Developer
* ASP.NET Core
* Microservices
* Clean Architecture
* Docker
* Kubernetes
* Azure DevOps

If you found this project useful, consider giving it a ⭐.
