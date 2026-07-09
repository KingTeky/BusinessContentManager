# Blossom Tree Manager - Full Plan

## Project Status Tracker
Last updated: 2026-07-09

| Workstream | Status | Notes |
|---|---|---|
| Solution scaffold (C# Blazor) | Done | `BlossomTreeManager.slnx` and `src/BlossomTreeManager.Web` created. |
| Auth and role policies | Done | Identity roles and authorization policies configured. |
| Domain model and EF mapping | Done | Core entities, relations, indexes, and constraints implemented. |
| Seed data | Done | Roles, demo school, users, room, student, attendance seeded. |
| Dashboard page | Done | Live summary and room cards from database. |
| Rooms page | Done | Data-backed room management view (read/list). |
| Users page | Done | Data-backed user list with role join. |
| Attendance page + CSV export | Done | Filtering and browser download implemented. |
| Settings page | Done | School settings load and save implemented. |
| Notification queue service | Done | Queue and mark-sent service layer added. |
| PWA/fullscreen baseline | Done | Manifest and fullscreen helper JS included. |
| PostgreSQL runtime wiring | Blocked | Current SDK is .NET 10; stable Npgsql provider mismatch with EF 10. SQLite used temporarily. |
| Multilingual support | Deferred | Explicitly postponed by request. |

## Product Scope
Blossom Tree Manager is a school check-in and administration platform for multi-role users:
- App Owner
- Company Staff
- School Administrator
- School Staff
- Teacher
- Parent
- Student (passive profile)

Core modules:
- Dashboard
- Rooms
- Users
- Attendance Logs
- School Settings
- Notifications

## Current Architecture (Implemented)
Stack:
- Frontend: ASP.NET Core Blazor Web App (Interactive Server)
- Auth: ASP.NET Core Identity + role-based policies
- Data access: Entity Framework Core
- Database provider now: SQLite (temporary runtime provider)
- Target database later: PostgreSQL

Key implementation paths:
- `src/BlossomTreeManager.Web/Program.cs`
- `src/BlossomTreeManager.Web/Data/ApplicationDbContext.cs`
- `src/BlossomTreeManager.Web/Data/ApplicationUser.cs`
- `src/BlossomTreeManager.Web/Data/Entities/*`
- `src/BlossomTreeManager.Web/Components/Pages/*`
- `src/BlossomTreeManager.Web/Services/*`

## Domain Model
Implemented entities:
- School
- ApplicationUser (extended identity user)
- StudentProfile
- ParentStudentLink
- Room
- RoomTeacherAssignment
- StudentEnrollment
- AttendanceRecord
- SchoolSetting
- NotificationMessage
- AuditLogEntry

Enums:
- AttendanceStatus
- NotificationChannel
- NotificationStatus

Multi-school design rule:
- School-scoped entities include `SchoolId`.
- Global roles (AppOwner, CompanyStaff) are policy-based and not restricted to one school.

## Authorization Model
Roles:
- AppOwner
- CompanyStaff
- SchoolAdmin
- SchoolStaff
- Teacher
- Parent

Policies currently registered:
- `AppOwnerOnly`
- `CompanyOrOwner`
- `SchoolAdminOnly`
- `SchoolAdminOrStaff`
- `TeacherOnly`
- `ParentOnly`

## Data and Seed Strategy
Startup behavior:
- App runs `Database.Migrate()` during startup through seed initializer.
- Seed creates roles and demo records if missing.

Seeded demo accounts:
- owner@blossomtree.app
- principal@sunnydale.edu
- johnson@sunnydale.edu
- john.parent@email.com

Demo password:
- ChangeMe123!

## UI Progress by Section
Dashboard:
- Room cards and user summary populated from live data.

Rooms:
- Room cards with teacher, capacity, student counts, present today.

Users:
- Joined user and role view by school.

Attendance:
- Date range and status filters.
- CSV export in browser.

Settings:
- Editable school settings persisted in database.

## Notifications
Current:
- Notification queue service writes pending notification records.
- Helper method to mark pending notifications as sent.

Pending:
- Background dispatcher and provider integrations (email/SMS/push).

## PWA and Fullscreen
Implemented:
- Web manifest in `wwwroot/manifest.webmanifest`.
- Helper JS in `wwwroot/js/app.js`.
- Fullscreen button on dashboard.

Pending:
- Service worker caching and offline strategy.

## Known Constraints and Risks
- PostgreSQL is the target but runtime is temporarily SQLite due to provider compatibility in this environment.
- SQLite transitive dependency warning exists (`NU1903` for `SQLitePCLRaw.lib.e_sqlite3`).

## Next Milestones
1. Add CRUD actions for Rooms, Users, Students, and Attendance.
2. Enforce tenant guardrails in service layer for all mutations.
3. Add background notification worker and channel adapters.
4. Move runtime provider to PostgreSQL when stable EF 10-compatible provider is available.
5. Add tests for domain rules, authorization, and critical queries.
6. Implement multilingual support later.