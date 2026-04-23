# .NET Backend - Hexagonal Architecture & Stored Procedures

This microservice is built on **.NET 10**, leveraging **Hexagonal Architecture (Ports and Adapters)** and **SOLID** principles. It utilizes **Dapper** for high-performance data access through **Stored Procedures**.

---

## Tech Stack

- **Runtime:** .NET 10.0
- **Data Access:** Dapper (Micro-ORM)
- **Database:** SQL Server
- **Architecture:** Hexagonal (Domain, Application, Infrastructure layers)
- **Persistence Strategy:** Stored Procedures for data-centric logic
- **Containerization:** Docker & Docker Compose

---

## Prerequisites

- **Docker** (Optional, recommended for SQL Server orchestration)
- **Git Bash**
- **.NET 10.0 SDK** (Required for local development and execution)

---

## Project Structure

📁 **Backend** 
 ├── 📁 **Domain** # Business entities and core logic (Zero external dependencies)  
 ├── 📁 **Application** # Use cases and Ports (Input/Output interfaces)  
 └── 📁 **Infrastructure** # Adapters (REST Controllers, Dapper Repositories, HTTP Clients)

---

## API Documentation (Swagger)

Once the service is running, you can access the documentation at:  
**Link:** [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## Getting Started

### Local Deployment
To spin up the environment and initialize the database, execute:

1. **Sql:**
   ```bash
   Execute folder Scripts/init-db.sql
   Execute folder Scripts/alter-db.sql
2. **Start:**
   ```bash
   dotnet run

### Docker Deployment
To spin up the environment and initialize the database, execute:

1. **Start containers:**
   ```bash
   docker-compose up -d --build
2. **Initialize the database schema:**
   ```bash
   MSYS_NO_PATHCONV=1 docker exec -it sqlserver_db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Passw0rd!' -C -i /docker-entrypoint-initdb.d/init-db.sql
2. **Alter the database schema:**
   ```bash
   MSYS_NO_PATHCONV=1 docker exec -it sqlserver_db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Passw0rd!' -C -i /docker-entrypoint-alterdb.d/alter-db.sql