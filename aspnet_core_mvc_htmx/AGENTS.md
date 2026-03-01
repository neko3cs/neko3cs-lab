# Project Context: AspNetCoreMvcHtmx

This project is a sample application that combines ASP.NET Core MVC with HTMX to achieve a dynamic UI (linked dropdowns) while minimizing JavaScript usage.

## Project Overview
- **Purpose**: Demonstrate how to dynamically swap PartialViews using HTMX while maintaining the MPA (Multi-Page Application) structure.
- **Main Technologies**:
    - ASP.NET Core MVC (Target Framework: .NET 10.0)
    - HTMX (via CDN)
    - Entity Framework Core (InMemory Database)
    - Bootstrap 5 (UI Styling)

## Architecture
- **Data Layer**: EF Core InMemoryDB using `AppDbContext`.
- **Data Seeding**: Loads nationwide prefecture and city data from `wwwroot/data/locations.json` at application startup.
- **UI Logic**: 
    - Triggered by prefecture change, it calls the `Cities` action via HTMX (`hx-get`) to fetch the city dropdown as a PartialView.
    - **OOB (Out-of-Band) Swaps**: Utilizes HTMX's `hx-swap-oob` feature to reset the selection label from the server side simultaneously with the city list update.
    - **JavaScript Zero**: Achieved full UI synchronization using only HTMX attributes and server-side PartialView control, without writing custom client-side scripts.

## Building and Running
- **Build**: `dotnet build`
- **Run**: `dotnet run` (Default ports: 5133/7133)
- **Tools**: `dotnet-ef` is configured as a local tool.

## Development Conventions
- **HTMX First**: Always consider if a dynamic UI change can be resolved with HTMX first to minimize JavaScript additions.
- **PartialView Usage**: Components that are dynamically changed should be extracted as independent PartialViews (`_*.cshtml`) and returned as such from controllers.
- **Data Management**: Data models are defined in `Models/Location.cs`. Prefecture IDs follow JIS order (1-47), and City IDs use the Ministry of Internal Affairs and Communications' local government codes.

## Key Files
- `AspNetCoreMvcHtmx/Program.cs`: Application configuration and DB seeding logic.
- `AspNetCoreMvcHtmx/Controllers/HomeController.cs`: Actions for handling HTMX requests (`Cities`, `SelectionLabel`).
- `AspNetCoreMvcHtmx/Views/Home/Index.cshtml`: Main view.
- `AspNetCoreMvcHtmx/Views/Home/_CitiesPartial.cshtml`: City dropdown including OOB update logic.
- `AspNetCoreMvcHtmx/wwwroot/data/locations.json`: Nationwide data source.
