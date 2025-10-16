# Sakura Sushi — WGU SE Capstone (2025)

A restaurant website with **menu & reservation CRUD**, **admin sign-in**, and **basic reporting**. Built with ASP.NET Core (Blazor Server), EF Core, and PostgreSQL. Containerized with Docker and deployed on Fly.io.

---

## Features

- **Public site**
  - Browse menu by category; item detail pages
  - Make, edit, cancel reservations (email confirmation optional)
- **Admin portal**
  - Secure sign-in (ASP.NET Core Identity)
  - Full CRUD for **Menu Items**, **Categories**, **Reservations**
  - **Reports**: daily/weekly reservations, revenue by menu category, top sellers
- **Engineering**
  - ASP.NET Core (Blazor Server) + EF Core + PostgreSQL
  - Dockerized dev & prod; one-command local run
  - Fly.io deployment with health checks
  - Seed data & admin bootstrap

---

## Tech Stack

- **Backend/UI:** ASP.NET Core (Blazor Server)
- **Data:** Entity Framework Core + PostgreSQL
- **Auth:** ASP.NET Core Identity (cookie auth)
- **Infra:** Docker / Docker Compose, Fly.io
- **Testing:** xUnit (unit) + minimal integration tests (EF Core in-memory)

---

## Quick Start (Local)

### 1) Prereqs
- .NET 8 SDK
- Docker + Docker Compose

### 2) Environment variables
Create a `.env` file in the repo root:

```env
# Database
POSTGRES_USER=sakura
POSTGRES_PASSWORD=changeme
POSTGRES_DB=sakura

# App (connection string used by EF Core)
ConnectionStrings__Default=Host=db;Database=sakura;Username=sakura;Password=changeme;Include Error Detail=true

# Seed admin (used once on first run)
SEED_ADMIN_EMAIL=admin@sakurasushi.local
SEED_ADMIN_PASSWORD=ChangeMe1!
```

### 3) Run with Docker (recommended)
```bash
docker compose up --build
# app: http://localhost:8080
```

### 4) First-run setup (auto)
- DB migrations apply at startup
- Seed data loads
- Admin user is created from env vars
