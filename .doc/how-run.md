# Running the Project (Docker Compose + Local API)

This project uses **PostgreSQL via Docker Compose** and runs the **.NET WebApi locally**.

---

## ✅ Requirements

- Docker Desktop
- .NET SDK 8+

---

## 🐳 1) Start Database with Docker Compose

From the project root, run:

```bash
docker-compose up -d
```
To check if containers are running:

```bash
docker ps
```


## 🐳 2) Apply Migrations (EF Core)

```bash
dotnet ef database update \
  --project ./src/Ambev.DeveloperEvaluation.ORM \
  --startup-project ./src/Ambev.DeveloperEvaluation.WebApi
```

## 🐳 3) Run the API

```bash
dotnet run --project ./src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj --launch-profile "http"
```


## 🐳 3) Run tests

```bash
dotnet test ./tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
```
