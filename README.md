# 🏥 School Medical Server

[![CI - Medicare](https://github.com/danielleit241/school-medical-server/actions/workflows/dotnet.yml/badge.svg)](https://github.com/danielleit241/school-medical-server/actions/workflows/dotnet.yml)

---

## 📝 Overview

School Medical Server is a backend system for managing school healthcare operations, including student health records, parent and staff accounts, medical registrations, inventory, appointments, notifications, and event management.

## 🛠️ Technologies Used

- **🖥️ Language:** C#
- **⚙️ Framework:** ASP.NET Core (.NET 8)
- **🗄️ ORM:** Entity Framework Core
- **🗃️ Database:** SQL Server (default, configurable)
- **🔒 Authentication:** JWT Bearer Authentication
- **📖 API Documentation:** Swagger (Swashbuckle)
- **✉️ Mail:** MailKit, MimeKit
- **📊 Excel Import/Export:** ClosedXML
- **🔎 Dynamic LINQ:** System.Linq.Dynamic.Core
- **🧪 Unit Testing:** xUnit, FluentAssertions, InMemory provider

## 🏗️ Functional Architecture

- **Domain Layer:** Contracts and entities (Abstractions)
- **Infrastructure Layer:** Data persistence, repositories, and services implementation
- **API Layer:** Dependency injection and API endpoints

**Key repositories and services include:**
- 👨‍🎓 User, Student, and Role management
- 📝 Medical Registration and Event tracking
- 📅 Appointment scheduling
- 💊 Medical Inventory management
- 🔔 Notifications and reporting

## 🚀 Installation Guide

### ✅ Prerequisites

- .NET 8 SDK or newer
- SQL Server (or configure another provider)
- Visual Studio 2022+ or VS Code with C# extensions

### 📦 Steps

1. **Clone the repository**
    ```bash
    git clone https://github.com/danielleit241/school-medical-server.git
    cd school-medical-server
    ```

2. **Restore dependencies**
    ```bash
    dotnet restore
    ```

3. **Configure the database connection**
    - Edit `appsettings.json` (in the API or Infrastructure project) and update the `ConnectionStrings` section with your SQL Server details.

4. **Apply database migrations**
    ```bash
    dotnet ef database update
    ```
    > If no migrations exist yet, create an initial migration:
    > ```bash
    > dotnet ef migrations add InitialCreate
    > dotnet ef database update
    > ```

5. **Run the application**
    ```bash
    dotnet run --project SchoolMedicalServer.Api
    ```
    - The API will usually be available at `https://localhost:700` or `http://localhost:5078` (see `launchSettings.json` for details).

6. **Test the API**
    - Use Swagger UI (usually at `/swagger`) or Postman to interact with endpoints.

## 🤝 Contribution

Pull requests and issues are welcome! Please submit your suggestions or bug reports via GitHub Issues.
