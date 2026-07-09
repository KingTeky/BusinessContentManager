# Blossom Tree Manager - Working Plan

## Summary
This file is the short working plan. For detailed implementation and tracker state, see `BTM Full Plan.md`.

Current implementation is a C# Blazor Web App with Identity, EF Core entities, seed data, and pages for Dashboard, Rooms, Users, Attendance, and Settings.

## Scope
- Multi-role school management and attendance tracking
- Role-based authorization
- School-scoped data model
- Notification queue baseline
- PWA/fullscreen baseline

Out of scope for now:
- Multilingual support (deferred)

## Architecture Decision
Chosen stack:
- ASP.NET Core Blazor Web App
- ASP.NET Core Identity
- Entity Framework Core

Database provider state:
- Running now: SQLite
- Intended later: PostgreSQL when EF 10 provider compatibility is stable

## Implemented Components
Application core:
- `src/BlossomTreeManager.Web/Program.cs`
- `src/BlossomTreeManager.Web/Data/ApplicationDbContext.cs`
- `src/BlossomTreeManager.Web/Data/ApplicationUser.cs`

Domain entities:
- School, Room, StudentProfile, ParentStudentLink
- RoomTeacherAssignment, StudentEnrollment, AttendanceRecord
- SchoolSetting, NotificationMessage, AuditLogEntry

Pages:
- `src/BlossomTreeManager.Web/Components/Pages/Home.razor`
- `src/BlossomTreeManager.Web/Components/Pages/Rooms.razor`
- `src/BlossomTreeManager.Web/Components/Pages/Users.razor`
- `src/BlossomTreeManager.Web/Components/Pages/Attendance.razor`
- `src/BlossomTreeManager.Web/Components/Pages/Settings.razor`

Services:
- `src/BlossomTreeManager.Web/Services/SchoolContext.cs`
- `src/BlossomTreeManager.Web/Services/DashboardService.cs`
- `src/BlossomTreeManager.Web/Services/AttendanceExportService.cs`
- `src/BlossomTreeManager.Web/Services/NotificationQueueService.cs`

PWA/fullscreen:
- `src/BlossomTreeManager.Web/wwwroot/manifest.webmanifest`
- `src/BlossomTreeManager.Web/wwwroot/js/app.js`

## Delivery Order Status
1. Auth and role policies: Done
2. Entities and EF migrations: Done
3. Rooms and Users pages: Done
4. Attendance with filters/export: Done
5. Notification queue foundation: Done
6. PWA/fullscreen baseline: Done
7. Multilingual support: Deferred

## Next Actions
1. Add CRUD operations for entities currently list-only.
2. Add background notification sender workers.
3. Add automated tests for authorization and tenant boundaries.
4. Switch runtime provider to PostgreSQL when stable compatible package is available.
