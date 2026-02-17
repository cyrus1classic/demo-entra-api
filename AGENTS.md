# AGENTS.md — Demo Entra (.NET Backend API)

This repository contains a demo ASP.NET Core Web API secured with Microsoft Entra ID (Azure AD).
Use this document as the operating guide for automated agents and human contributors.

---

## 1) Goals & Non-Goals

### Goals
- Provide a minimal, clear example of:
  - JWT Bearer authentication using Microsoft Entra ID
  - Role/scopes-based authorization
  - A couple of protected endpoints returning user identity info
- Be easy to run locally and in CI.

### Non-Goals
- No full production hardening (WAF, rate limits, advanced logging/observability).
- No complex multi-tenant onboarding UX (kept minimal for demo).

---

## 2) Tech Stack
- .NET 8 (ASP.NET Core Web API)
- Microsoft.Identity.Web (auth middleware)
- Swagger/OpenAPI for local testing

---

## 3) Repository Layout (expected)
- `src/DemoApi/` — ASP.NET Core API
  - `Program.cs`
  - `appsettings.json`
  - `Controllers/`
  - `Auth/` (if needed for helpers/policies)
- `tests/DemoApi.Tests/` — unit/integration tests
- `docs/` — any diagrams / extra notes

If the structure differs, update this AGENTS.md.

---

## 4) Local Setup

### Prereqs
- .NET SDK 8+
- An Entra App Registration (see `docs/entra-setup.md` if present)

### Environment variables
Create a `.env` or set environment variables in your shell:

- `Entra__TenantId` — Directory (tenant) ID
- `Entra__ClientId` — Application (client) ID
- `Entra__Audience` — Usually same as ClientId (demo), or `api://{clientId}`
- `Entra__Instance` — `https://login.microsoftonline.com/`

Optional (if using downstream API calls):
- `Entra__ClientSecret` — only if required for daemon calls (not recommended for pure API demo)

> Do not commit secrets. If you see secrets in commits, remove and rotate them.

### Run
- `dotnet restore`
- `dotnet build`
- `dotnet run --project src/DemoApi/DemoApi.csproj`

Swagger should be available at:
- `https://localhost:<port>/swagger`

---

## 5) Authentication & Authorization Rules

### JWT validation
- The API accepts access tokens issued by Entra ID.
- Validate:
  - issuer: `https://login.microsoftonline.com/{TenantId}/v2.0`
  - audience: `Entra__Audience` (or `api://{ClientId}`)

### Authorization model (demo)
- Default: endpoints require authentication unless explicitly `[AllowAnonymous]`.
- Use either:
  - **Scopes** (delegated permissions) via `scp` claim, e.g. `access_as_user`
  - **App roles** via `roles` claim, e.g. `Demo.Read`, `Demo.Admin`

**Policy naming convention**
- `Scopes:<scope>` for delegated permissions
- `Roles:<role>` for app roles

Examples:
- `RequireScope("access_as_user")`
- `RequireRole("Demo.Admin")`

---

## 6) Endpoints (demo contract)

### Public
- `GET /health` — returns `200 OK` (no auth)

### Protected
- `GET /me` — returns basic claims for the caller
- `GET /secure/read` — requires scope `access_as_user` OR role `Demo.Read`
- `POST /secure/admin` — requires role `Demo.Admin`

When adding endpoints:
- Keep them small and focused.
- Add authorization attributes/policies explicitly.
- Update this section if you add/remove endpoints.

---

## 7) Coding Standards & Conventions

### Style
- Prefer small controllers; move logic into services.
- Avoid introducing heavy frameworks for a demo.
- Keep configuration strongly-typed: `IOptions<T>`.

### Error handling
- Return Problem Details (RFC 7807) for errors.
- Don’t leak token contents in logs.

### Logging
- Log:
  - request id
