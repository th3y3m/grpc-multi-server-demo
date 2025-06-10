[![CodeQL](https://github.com/th3y3m/grpc-multi-server-demo/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/th3y3m/grpc-multi-server-demo/actions/workflows/github-code-scanning/codeql)
![MIT License](https://img.shields.io/badge/License-MIT-yellow.svg)


![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=dot-net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)

# 🛒 Category & Product Service Architecture

Welcome to the **Category & Product Service**!  
This document explains the system architecture and how all components work together to provide a high-performance, scalable product and category management solution.

---

## 🖼️ System Overview

![Architecture Diagram](https://github.com/th3y3m/grpc-multi-server-demo/blob/main/Diagrams/diagram.png)

---

## 🚦 Flow Overview

1. **Clients** (Web/Mobile) send HTTP/JSON requests.
2. **API Gateway** routes requests, applies middleware, and handles business logic.
3. **CategoryBusiness** checks the Redis cache for data.
4. On cache miss, it calls backend services via gRPC.
5. **gRPC Services** process product and category requests.
6. **Data Layer** persists information in the databases.

---

## 🏛️ Core Components

### 🟩 API Gateway

- **Entry point** for all client requests.
- **Components:**
  - `ErrorWrappingApiMiddleware`: Ensures consistent error handling.
  - `CategoryController`: Handles category HTTP endpoints.
  - `CategoryBusiness`: Core business logic.
  - `CacheRedis`: Interacts with Redis for fast data retrieval.
  - `CategoryServiceProxy`: Calls gRPC services if cache misses.

---

### 🟧 Redis Cache

- **Purpose:** In-memory cache for quick category lookups.
- **Flow:**
  - `CategoryBusiness` checks `CacheRedis`.
  - **Cache hit:** Returns data instantly.
  - **Cache miss:** Calls gRPC service, updates cache.

---

### 🔷 Product & Category gRPC Services

- **ProductServiceImpl** and **CategoryServiceImpl** handle business logic.
- **Commands & Handlers:** Manage create/update/delete actions.
- **Queries & Handlers:** Manage fetching and listing operations.
- **Handlers** interact with the database via entities and repositories.

---

### 🟪 Shared gRPC Contracts

- **Protobuf Schemas:** Define messages for gRPC communication.
- **GenericRepository & UnitOfWork:** Generic data access and transaction management.
- **Repository/UoW Interfaces:** Define contracts for data operations.

---

### 🗄️ Data Layer

- **Entities:** `Product Entity`, `Category Entity`
- **Persistence:** `AppDbContext` handles data storage.
- **Databases:** `ProductDB`, `CategoryDB`

---

## 🌐 Technologies Used

- **API Gateway:** ASP.NET Core (Middleware, Controllers)
- **Caching:** Redis
- **gRPC:** High-performance RPC protocol
- **Entity Framework Core:** ORM for data persistence
- **Protobuf:** Contract definitions for gRPC
- **Domain-Driven Design:** Commands, Queries, Repositories

---

## 🚀 Key Features

- **Blazing Fast:** Redis caching for instant lookups.
- **Scalable:** Microservices with gRPC.
- **Maintainable:** Commands, queries, repositories for clear structure.
- **Extensible:** Shared contracts and generic repositories for easy expansion.

---

## 🎨 Color Legend

| Color        | Component Type         |
|--------------|-----------------------|
| 🟩 Green     | API Gateway Layer     |
| 🟧 Orange    | Caching Layer         |
| 🔷 Blue      | gRPC Services Layer   |
| 🟪 Purple    | Shared Contracts      |
| 🗄️ Grey/White | Database Layer        |

---

## 🧭 How to Extend

- **Add endpoints:** Implement new actions in `CategoryController` or `ProductServiceImpl`.
- **Add caching:** Use `CacheRedis` for new entity caching.
- **Support new data:** Extend repository interfaces and update `AppDbContext`.

---

> **Happy Coding! 🚀**