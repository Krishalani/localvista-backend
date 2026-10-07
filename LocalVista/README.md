# LocalVista API

ASP.NET Core API for the Local Tourist Day-Visit Planner. Uses the existing SQL Server `LocalVista` catalogue tables (from `database/LocalVista_Schema.sql`) plus ASP.NET Core Identity for admin auth.

## Configure

`appsettings.json`:

- `ConnectionStrings:DefaultConnection` → `(localdb)\mssqllocaldb;Database=LocalVista;...`
- `Cors:Origins` → Angular origins (`http://localhost:4200`)
- `AdminSeed` → development admin (`manager` / `Manager123`)

Catalogue seed data is **not** overwritten by the API. Startup only:

1. Applies Identity migrations (AspNet* tables)
2. Ensures Admin role + seeded admin user exist

## Run with Angular

```bash
# Terminal 1 — API
cd LocalVista/LocalVista
dotnet run

# Terminal 2 — UI
cd localvista-tourism-platform/localvista-tourism-platform/local-tourist-system
npm start
```

- API Swagger: https://localhost:7156/swagger  
- Angular: http://localhost:4200  
- Set `src/environments/environment.ts` `apiBaseUrl` if the API port differs.

## Endpoints

| Method | Path | Auth |
|--------|------|------|
| GET | `/api/categories` | Public |
| GET | `/api/attractions?search=&categories=` | Public |
| GET | `/api/attractions/{id}` | Public |
| POST | `/api/auth/login` | Public |
| POST | `/api/auth/logout` | Authenticated |
| GET | `/api/auth/me` | Cookie session |
| POST | `/api/attractions` | Admin |
| PUT | `/api/attractions/{id}` | Admin |
| DELETE | `/api/attractions/{id}` | Admin |
