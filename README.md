# IP Geo Web – Backend

An ASP.NET Core Web API for IP geolocation lookup, user authentication, and history tracking.

## Features

- REST API for IP geolocation and history
- PostgreSQL database with Entity Framework Core
- JWT authentication and ASP.NET Identity
- AutoMapper for DTO mapping
- Swagger/OpenAPI documentation

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL database

### Setup

1. Navigate to this `backend/` directory
2. Configure your connection string and `IpGeoToken` in `src/IpGeoApi/appsettings.json`
3. Run database migrations:
   ```
   dotnet ef database update --project src/IpGeoApi/IpGeoApi.csproj
   ```
4. Start the API:
   ```
   dotnet run --project src/IpGeoApi/IpGeoApi.csproj
   ```

## Project Structure

- `src/IpGeoApi/` — Main API project
- `Controllers/` — API controllers
- `Data/` — Database context and migrations
- `Services/` — Business logic
- `DTOs/` — Data transfer objects

## Environment Variables

- `IpGeoToken` — API token for IP geolocation service

## License

MIT
