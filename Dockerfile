# ==============================
# Build Stage
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /out

# ==============================
# Runtime Stage
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output
COPY --from=build /out .

# Expose port
EXPOSE 8080

# Run application
ENTRYPOINT ["dotnet", "IpGeo.Api.dll"]