# Blossom Tree Manager

Blossom Tree Manager is a school check-in and administration platform currently implemented as a C# Blazor Web App.

## Current State
Status date: 2026-07-09

Implemented:
- C# Blazor solution scaffold
- Identity authentication and role policies
- EF Core domain model and migrations
- Seed data for demo school and users
- Tenant mutation guard service with audit logging hooks
- Dashboard page with live summary
- Rooms page with guarded CRUD
- Users page with guarded role/status mutations (SchoolAdmin/AppOwner)
- Students page with guarded student and enrollment management
- Attendance page with filters, CSV export, and guarded check-in/check-out
- Settings page with save support
- Notification queue service with background dispatcher worker
- Automated authorization, tenant-boundary, and notification lifecycle tests (9 passing)
- PWA manifest and fullscreen helper JS

Deferred:
- Multilingual support

Known constraint:
- Runtime provider is SQLite for now due to EF Core 10 and stable PostgreSQL provider compatibility in this environment.

## Tech Stack
- .NET 10
- ASP.NET Core Blazor Web App
- ASP.NET Core Identity
- Entity Framework Core
- SQLite (temporary runtime provider)

## Project Structure
```text
BlossomTree_Manager/
├── BlossomTreeManager.slnx
├── BTM Full Plan.md
├── BTM.md
├── mockup.html
├── manifest.json
├── README.md
└── src/
	└── BlossomTreeManager.Web/
```

## Run the Application
From repository root:

```powershell
dotnet restore BlossomTreeManager.slnx
dotnet build BlossomTreeManager.slnx
dotnet run --project src/BlossomTreeManager.Web/BlossomTreeManager.Web.csproj
```

## Demo Seed Accounts
- owner@blossomtree.app
- principal@sunnydale.edu
- johnson@sunnydale.edu
- john.parent@email.com

Password:
- ChangeMe123!

## Documentation
- Detailed roadmap and progress tracker: `BTM Full Plan.md`
- Short implementation plan: `BTM.md`

## Next Steps
1. Add component/integration tests for page-level mutation actions (Rooms, Students, Attendance, Users UI interactions).
2. Move runtime provider to PostgreSQL when stable compatible package is available.
