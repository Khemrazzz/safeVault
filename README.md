# SafeVault – Secure Application Development

SafeVault demonstrates core secure coding techniques for protecting user data in an ASP.NET Core environment. The solution consists of a reusable security-focused class library, an ASP.NET Core Web API wired with Identity and role-based access control (RBAC), and a set of unit tests that exercise the critical security guarantees.

## Features

- **Input validation** – `InputValidator` guards against malformed email input with a strict regular expression check.
- **SQL Injection resistance** – `UserRepository` uses parameterised queries (via Dapper) and a factory-based connection pattern so that malicious strings such as `' OR 1=1 --` cannot escape the intended query.
- **Cross-site scripting (XSS) hardening** – `CommentSanitizer` encodes untrusted user-generated HTML before rendering.
- **Password policy enforcement** – `PasswordPolicy` enforces OWASP-aligned length and character diversity requirements.
- **Authentication and RBAC** – ASP.NET Identity with an in-memory Entity Framework Core store powers authentication. The `UsersController` exposes a secured admin-only endpoint that creates new accounts and assigns the default `User` role.
- **Role seeding** – Roles are created automatically at application startup to ensure RBAC checks succeed on first run.

## Project layout

```
SafeVault.Core   – Shared security primitives and the data access layer
SafeVault.Api    – ASP.NET Core Web API secured with Identity and RBAC
SafeVault.Tests  – MSTest project that validates security behaviour
```

## Running the tests

```bash
dotnet test
```

The test suite verifies email validation behaviour, ensures that SQL injection attempts fail against the repository, validates HTML encoding, and checks that weak passwords are rejected.
