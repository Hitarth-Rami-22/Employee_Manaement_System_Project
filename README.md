Employee Management System

📌 Project Overview
The Employee Management System is a .NET Core-based Web API that provides role-based authentication and authorization
for employees and admins. It includes functionalities for employee management, timesheet tracking, leave management, 
and reporting while ensuring security, logging, and error handling.

✅ Implemented Features
🔹 Authentication & Authorization
JWT Authentication: Secure login system using JSON Web Tokens.
Role-Based Access Control (RBAC): Restricts API access based on user roles (Admin, Employee).

🔹 Employee Management
Add, Update, Delete Employees
Update Employee Profile (Phone, Address, Tech Stack)
Password Reset Functionality

🔹 Timesheet Management
Employees log work hours daily.
Admins can approve/reject timesheets.
Export timesheets to Excel for reporting.

🔹 Leave Management
Employees can apply for leave.
Admins can approve/reject leave requests.
Leave approval tracking (approved by, timestamp).

🔹 Pagination & Filtering
Implemented pagination (page, pageSize) for Employee, Leave, and Timesheet APIs.
Filtering options (department, tech stack, date, status).

🔹 Error Handling & Logging
Centralized Exception Handling Middleware.
Structured API Error Responses for better debugging.
Logs authentication failures, API errors, and approvals/rejections.

❌ Remaining Features
🔸 Reports & Analytics (Not Implemented)
Admin should be able to generate reports for work hours (weekly, monthly).
Report visualization using charts/graphs (Optional).

🔸 Email Notifications (Not Implemented)
Send email notifications when an employee’s leave request is approved/rejected.
Send reminders for pending timesheets.

🔸 Admin Dashboard (Not Implemented)
Display employee activities, pending approvals, and analytics.
Provide an overview of pending vs. approved leaves & timesheets.

🔸 Two-Factor Authentication (2FA) (Not Implemented)
Add optional 2FA security for Admin login.

🔸 Attendance Tracking System (Not Implemented)
Employees log their daily attendance separately (Optional Feature).

💻 Technologies Used
.NET Core Web API
Entity Framework Core (Code-First Approach)
JWT Authentication
AutoMapper (DTO Mapping)
Microsoft SQL Server
Serilog & ILogger (Logging)
OfficeOpenXml (Excel Export)
