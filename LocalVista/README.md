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

- API HTTP: http://localhost:5088 (Swagger HTTPS: https://localhost:7088/swagger)  
- Angular: http://localhost:4200 — proxies `/api` → `http://localhost:5088` via `proxy.conf.json`  
- Restart both `dotnet run` and `npm start` after port/proxy changes.

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
