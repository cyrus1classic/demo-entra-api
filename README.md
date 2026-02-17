# Demo Entra API

A minimal ASP.NET Core Web API example secured with Microsoft Entra ID.

## Endpoints

- `GET /health` (anonymous)
- `GET /me` (authenticated)
- `GET /secure/read` (requires `access_as_user` scope or `Demo.Read` role)
- `POST /secure/admin` (requires `Demo.Admin` role)

## Run

```bash
dotnet restore
dotnet build
dotnet run --project src/DemoApi/DemoApi.csproj
```

Swagger is available in development at `/swagger`.
