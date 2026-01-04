# Task Management - Backend API
RESTful API built with .NET Core for task management application.

## Features

- JWT authentication
- BCrypt password hashing
- PostgreSQL database with Entity Framework Core
- Three-layer architecture (Controllers, Services, Repositories)
- CORS configuration

## Tech Stack

- .NET Core 10
- PostgreSQL
- Entity Framework Core
- JWT Authentication
- BCrypt

## Installation
```bash
dotnet restore
dotnet run
```

## Configuration

Update `appsettings.json` with your connection string and JWT key.

## API Endpoints

### Auth
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Todos
- `GET /api/todos` - Get all todos (authenticated)
- `POST /api/todos` - Create todo (authenticated)
- `PUT /api/todos/{id}` - Update todo (authenticated)
- `DELETE /api/todos/{id}` - Delete todo (authenticated)
