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
