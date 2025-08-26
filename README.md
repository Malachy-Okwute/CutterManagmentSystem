# 🛠️ Cutter Management System

## Overview  
The **Cutter Management System** is a desktop application built with WPF and .NET, designed to streamline the management of industrial machines and their associated cutting tools. It also facilitates logging the condition of output parts—representing the physical results of machine operations—to support traceability and quality assurance across production workflows.

---

## ✨ Features

- **Machine Registry**  
  Maintain a structured inventory of machines with metadata such as model, location, and operational status.

- **Cutter Association**  
  Assign cutters to machines, track usage cycles, and manage cutter transitions across production stages.

- **Part Condition Logging**  
  Record the condition of output parts (not machine components), enabling traceability and post-process evaluation.

- **User-Driven Configuration**  
  Connection strings and runtime settings are collected via intuitive prompts and persisted securely in JSON format.

- **Data Persistence**  
  Supports local database integration with runtime-configurable connection strings and layered configuration loading.

- **Extensible Architecture**  
  Designed with clean separation of concerns, MVVM pattern, and DI-ready components for future scalability.

---

## 🧱 Technical Highlights

| Area                     | Details                                                                  |
|--------------------------|--------------------------------------------------------------------------|
| **Frontend**             | WPF with MVVM, custom controls, and input dialogs                        |
| **Backend**              | .NET Core, Entity Framework Core, layered config loading                 |
| **Persistence**          | SQL Server (runtime-configurable), JSON-based user settings              |
| **Configuration**        | First-launch prompts, user overrides, and secure storage of settings     |
| **CI/CD**                | GitHub Actions, versioning automation, rationale-driven release workflow |
| **Documentation**        | Annotated setup guides, config rationale, and user messaging strategy    |

---

## 🔐 Configuration Flow - (Coming soon)

On first launch, users are prompted to enter database connection details (e.g., server, database name, authentication method). These are validated, tested, and saved to a local JSON file for future use. The system supports both trusted connections and SQL authentication, with optional encryption for sensitive fields.

---

## 📦 Future Enhancements

- Prompt users to enter database connection details (e.g., server, database name, authentication method)

- Role-based access control for multi-user environments

- Integration with barcode scanning for cutter tracking

- Exportable reports for part condition summaries

- Cloud sync for distributed machine management
