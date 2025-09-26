
Absolutely, Eli — here’s the **complete, structured Markdown version** of our entire conversation so far, from your very first prompt to this moment. It’s formatted for Obsidian or any Markdown editor, preserving all tables, wireframes, diagrams, and planning logic.

---

# 🌳 Blossom Tree Manager – Full Planning Session

## 🧠 Initial Prompt

> Help me plan a school check-in application (let's call this app: Blossom Tree Manager, and Blossom Tree is also the name of the company) with the following features...

## ✅ Feature Blueprint

### Core Entities

|Entity|Description|
|---|---|
|**School Account**|Represents a registered school. Each school has its own users, rooms, and settings.|
|**User**|Can be a Teacher, Parent/Guardian, Kid, School Staff, School Administrator, Company Staff, or App Owner.|
|**Room**|Represents a classroom with assigned grade level and teacher(s).|

### Authentication & Roles

#### Sign-In System

- School Account Sign-In
- User Sign-In (role-based):
    - Teachers
    - Parents/Guardians
    - Kids (passive)
    - School Staff
    - School Administrator
    - Company Staff
    - App Owner

#### Role-Based Access Control (RBAC)

|Role|Permissions|
|---|---|
|**App Owner**|Full access to all schools, users, settings|
|**Company Staff**|Limited access to school-level data|
|**School Admin**|Full control over their school|
|**School Staff**|Partial admin rights|
|**Teacher**|Room-level access|
|**Parent/Guardian**|Child-level access|
|**Kid**|Passive profile|

---

## 🔄 User Flows by Role

### App Owner

- Sign In → Dashboard
- View/manage all schools
- Add/remove company staff
- Audit logs
- Override school settings

### Company Staff

- Sign In → Company Dashboard
- View school accounts
- Assist with setup
- Cannot manage other company staff or app owner

### School Administrator

- Sign In → School Admin Dashboard
- Create/edit rooms
- Assign users
- Approve parent accounts
- Manage school settings

### Teacher

- Sign In → Teacher Dashboard
- View assigned rooms/students
- Mark attendance
- Communicate with parents

### School Staff

- Sign In → Staff Dashboard
- View student list
- Assist with check-in/out
- Limited edit rights

### Parent/Guardian

- Sign In → Parent Dashboard
- View child’s check-in status
- Communicate with teachers
- Request updates
- Link/unlink children (admin approval)

### Kid

- Passive account
- Linked to parent and room

---

## 🧱 Role Hierarchy Diagram

```plaintext
App Owner (Full Control)
└── Company Staff (Limited Global Control)
    └── School Administrator (Full School Control)
        └── School Staff (Partial School Control)
            └── Teacher (Room-Level Control)
                └── Parent/Guardian (Child-Level Access)
                    └── Kid (Passive Profile)
```

---

## 🔐 Permissions Matrix

|Action|App Owner|Company Staff|School Admin|School Staff|Teacher|Parent|Kid|
|---|---|---|---|---|---|---|---|
|View all schools|✅|✅ (limited)|❌|❌|❌|❌|❌|
|Manage school accounts|✅|❌|❌|❌|❌|❌|❌|
|Add/remove company staff|✅|❌|❌|❌|❌|❌|❌|
|Create/edit rooms|✅|❌|✅|❌|❌|❌|❌|
|Assign users to roles|✅|❌|✅|❌|❌|❌|❌|
|View room roster|✅|✅|✅|✅|✅|❌|❌|
|Mark attendance|✅|✅|✅|✅|✅|❌|❌|
|Communicate with parents|✅|✅|✅|✅|✅|✅|❌|
|View child check-in status|✅|✅|✅|✅|✅|✅|❌|
|Link/unlink children|✅|✅|✅|❌|❌|Request|❌|
|View/edit global settings|✅|❌|❌|❌|❌|❌|❌|

---

## 🧭 Wireframes

### App Owner Dashboard

```
┌────────────────────────────────────────────┐
│ Blossom Tree Manager - App Owner Dashboard│
├────────────────────────────────────────────┤
│ Sidebar:                                   │
│ - Schools                                  │
│ - Company Staff                            │
│ - Audit Logs                               │
│ - Global Settings                          │
│ - Logout                                   │
├────────────────────────────────────────────┤
│ Main Panel:                                │
│ [Search Schools] [Add School]              │
│ ┌────────────┬────────────┬────────────┐   │
│ │ School A   │ School B   │ School C   │   │
│ └────────────┴────────────┴────────────┘   │
│ [Recent Activity Logs]                     │
└────────────────────────────────────────────┘
```

### School Admin Dashboard

```
┌────────────────────────────────────────────┐
│ School Admin Dashboard - [School Name]     │
├────────────────────────────────────────────┤
│ Sidebar:                                   │
│ - Rooms                                    │
│ - Users                                    │
│ - Attendance Logs                          │
│ - Settings                                 │
│ - Logout                                   │
├────────────────────────────────────────────┤
│ Main Panel:                                │
│ [Add Room] [Add User]                      │
│ ┌────────────┬────────────┬────────────┐   │
│ │ Room 101   │ Room 102   │ Room 103   │   │
│ └────────────┴────────────┴────────────┘   │
│ [User Summary]                             │
└────────────────────────────────────────────┘
```

### Teacher Dashboard

```
┌────────────────────────────────────────────┐
│ Teacher Dashboard - [Teacher Name]         │
├────────────────────────────────────────────┤
│ Sidebar:                                   │
│ - My Rooms                                 │
│ - Students                                 │
│ - Messages                                 │
│ - Attendance                               │
│ - Logout                                   │
├────────────────────────────────────────────┤
│ Main Panel:                                │
│ [Room: 1st Grade - Room 101]               │
│ ┌────────────┬────────────┬────────────┐   │
│ │ Student A  │ Student B  │ Student C  │   │
│ └────────────┴────────────┴────────────┘   │
│ [Check-In Panel]                           │
└────────────────────────────────────────────┘
```

### Room Management View

```
┌────────────────────────────────────────────┐
│ Room Management - [Room Name]              │
├────────────────────────────────────────────┤
│ [Edit Room Info] [Assign Teacher]          │
│ Grade Level: 2nd Grade                     │
│ Assigned Teacher: Ms. Rivera               │
│ ┌────────────┬────────────┬────────────┐   │
│ │ Student A  │ Student B  │ Student C  │   │
│ └────────────┴────────────┴────────────┘   │
│ [Add Student] [View Attendance Logs]       │
└────────────────────────────────────────────┘
```

---

## 📣 Notification System

### Channels

|Channel|Use Cases|
|---|---|
|Email|Account creation, daily summaries|
|SMS|Urgent alerts, verification codes|
|Push|Real-time updates, messages|

### Tables

#### `notifications`

```sql
CREATE TABLE notifications (
  id UUID PRIMARY KEY,
  sender_id UUID REFERENCES users(id),
  receiver_id UUID REFERENCES users(id),
  message TEXT NOT NULL,
  channel ENUM('email', 'sms', 'push') NOT NULL,
  status ENUM('pending', 'sent', 'failed') DEFAULT 'pending',
  sent_at TIMESTAMP,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### `user_preferences`

```sql
CREATE TABLE user_preferences (
  user_id UUID PRIMARY KEY REFERENCES users(id),
  email_opt_in BOOLEAN DEFAULT TRUE,
  sms_opt_in BOOLEAN DEFAULT FALSE,
  push_opt_in BOOLEAN DEFAULT TRUE,
  quiet_hours_start TIME,
  quiet_hours_end TIME
);
```

---

## 🌐 Multilingual Support (English & Spanish)

### Folder Structure

```plaintext
/locales
  └── en/translation.json
  └── es/translation.json
```

### Sample Keys

```json
{
  "dashboard.title": "Welcome to Blossom Tree Manager",
  "room.grade": "Grade Level",
  "checkin.success": "Check-in successful"
}
```

```json
{
  "dashboard.title": "Bienvenido a Blossom Tree Manager",
 
```