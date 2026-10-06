# L'Escapade Ardennaise – Booking App

Website and booking system for **L'Escapade Ardennaise**, a holiday home in Rocquigny (French Ardennes).
Visitors can explore the house, request a stay and follow the status of their request. The owners manage all requests through an admin dashboard.

## Features

**Visitors**
- Information pages: the house, rooms, facilities, prices, the Rocquigny area and contact
- Booking request with an inline calendar (date range picker) and a confirmation dialog
- Confirmation email with a personal tracking link
- Status page per booking via a unique token (`/booking/trackbooking?token=...`)
- Multilingual: Dutch (default), French and English

**Admin**
- Login for the owners
- Dashboard with upcoming and past bookings
- Approve or delete bookings

## Tech stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core MVC (.NET 10) |
| Data | Entity Framework Core 10 + SQL Server |
| Frontend | Razor Views, Bootstrap 5, custom CSS, vanilla JS, [flatpickr](https://flatpickr.js.org/) |
| Email | SMTP via `System.Net.Mail` (Mailtrap in development) |
| Localization | `IViewLocalizer` + `.resx` resources |
| Deployment | Docker (multi-stage build) |

## Project structure

```
Escapade.Booking/
├── Escapade.Booking.Core/          # Domain and data layer
│   ├── Data/                       # EscapadeDbContext, DataSeeder
│   ├── Entities/                   # Booking, User, Role
│   └── Services/                   # BookingService, EmailService, UserService
│       ├── Interfaces/
│       └── Models/                 # ResultModel, request models
├── Escapade.Booking.Web/           # Presentation layer
│   ├── Controllers/                # Home, About, Booking, Contact, Admin, Language
│   ├── ViewModels/
│   ├── Views/
│   ├── Resources/                  # Translations (.resx)
│   └── wwwroot/                    # CSS, JS, images, video
└── Dockerfile
```

The Web layer only talks to the Core services through interfaces (`IBookingService`, `IUserService`, `IEmailService`). Services return a `ResultModel<T>` containing `IsSuccess`, `Items` and `Errors`.

<<<<<<< HEAD
=======
## Getting started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or via Docker)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### 1. Clone the repository

```bash
git clone https://github.com/dylanhrz/escapade-booking-app.git
cd escapade-booking-app/Escapade.Booking
```

### 2. Start SQL Server (optional, via Docker)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrongPassword>" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Configure User Secrets

Keep sensitive data out of `appsettings.json` and store it in User Secrets instead:

```bash
cd Escapade.Booking.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=EscapadeDb;User Id=sa;Password=<YourStrongPassword>;TrustServerCertificate=True;"
dotnet user-secrets set "Smtp:Host" "sandbox.smtp.mailtrap.io"
dotnet user-secrets set "Smtp:Port" "2525"
dotnet user-secrets set "Smtp:Username" "<mailtrap-username>"
dotnet user-secrets set "Smtp:Password" "<mailtrap-password>"
dotnet user-secrets set "Smtp:FromAddress" "info@lescapadeardennaise.be"
dotnet user-secrets set "Smtp:FromName" "L'Escapade Ardennaise"
```

### 4. Create the database

```bash
cd ..
dotnet ef migrations add InitialCreate --project Escapade.Booking.Core --startup-project Escapade.Booking.Web
dotnet ef database update --project Escapade.Booking.Core --startup-project Escapade.Booking.Web
```

> The `DbContext` lives in the Core project. To generate migrations there, Core also needs the `Microsoft.EntityFrameworkCore.Relational` package.

### 5. Run

```bash
dotnet run --project Escapade.Booking.Web
```

The app runs on `https://localhost:7148` or `http://localhost:5221`. The admin area is available at `/admin`.

## Docker

```bash
cd Escapade.Booking
docker build -t escapade-booking .
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="<connection-string>" \
  -e Smtp__Host="..." -e Smtp__Port="587" \
  -e Smtp__Username="..." -e Smtp__Password="..." \
  -e Smtp__FromAddress="..." -e Smtp__FromName="L'Escapade Ardennaise" \
  escapade-booking
```

## Booking flow

1. The visitor selects a date range and fills in the form.
2. The booking is saved with status `Pending` and a unique `AccessToken` (GUID).
3. The visitor receives an email with the tracking link.
4. The admin approves (`Approved`) or deletes the booking.
5. The visitor follows the status on the tracking page.

## Roadmap

- [ ] Hash passwords and secure the admin area with cookie authentication and `[Authorize]`
- [ ] Add EF Core migrations to the repository
- [ ] Reject bookings (`Rejected`) from the dashboard
- [ ] Show booking status in the admin dashboard
- [ ] Prevent overlapping bookings and show unavailable dates in the calendar
- [ ] Fully translate the booking and admin pages (FR/EN)
- [ ] Unit tests for the services
