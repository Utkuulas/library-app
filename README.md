# 📚 LibraryApp

LibraryApp is an ASP.NET MVC web application that allows users to manage and borrow books. It includes authentication, role-based authorization, email verification, and a user-friendly interface for both administrators and members.

## 🚀 Technologies Used

- ASP.NET MVC
- Entity Framework (EF)
- Identity (Authentication & Authorization)
- MSSQL LocalDB
- Mailtrap (for email verification and password reset)

## 👥 User Roles

The system defines two roles by default:
- **Admin**
- **Member**

These roles are configurable in the `appsettings.json` file under the `RoleSettings` section. New roles can be added as needed.

Users who register with email addresses listed under `AdminSettings:AdminEmails` are automatically assigned the **Admin** role. All other users are assigned the **Member** role by default.

---

## 🔐 Features

### Member Role
- View and search the list of available books.
- Borrow books if they are available (`isAvailable = true`).
- View book details.

### Admin Role
- View and search the list of available books.
- Add new books to the database.
- Edit book details.
- View book details.
- Mark returned books as "received", making them available again for other members.

---

## 🛠️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/LibraryApp.git
cd LibraryApp
```
### 2. Clone the Repository

Make sure you have SQL Server LocalDB installed (comes with Visual Studio).

Run the following commands in the Package Manager Console to apply migrations:
```bash
Update-Database
```

You can specify the context if needed:
```bash
Update-Database -Context YourDbContextName
```

### 3. Configure appsettings.json

Update the following sections in appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
},
"RoleSettings": {
  "Roles": [ "Admin", "Member" ]
},
"AdminSettings": {
  "AdminEmails": [ "admin@example.com", "john@library.com" ]
},
"EmailSettings": {
    "SmtpServer": "sandbox.smtp.mailtrap.io",
    "Port": 587,
    "SenderName": "Library App",
    "SenderEmail": "noreply@libraryapp.com",
    "Username": "your-username",
    "Password": "your-password"
  },
"AllowedHosts": "*"
```

You must create a Mailtrap account and obtain the SMTP credentials for email functionality (confirmation, password reset, etc.).

### 4. Run the Application

In Visual Studio:

- Set the startup project to LibraryApp.
- Press F5 or click Start Debugging.

The application should launch in your browser.

🧪 Testing the App
- Register with an email listed in AdminEmails to test Admin features.
- Register with any other email to test Member features.
- Try actions like borrowing books, adding/editing books, and email verification.

💡 Notes
- Default Identity structure is used for user management.
- All sensitive settings (e.g., SMTP credentials) should be stored securely in a production environment (e.g., environment variables or user secrets).
- Consider seeding initial book and user data for demo/testing purposes.
