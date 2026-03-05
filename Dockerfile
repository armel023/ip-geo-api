# ==============================
# Build Stage
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY IpGeoApi.slnx ./
COPY src/IpGeoApi/IpGeoApi.csproj ./src/IpGeoApi/
RUN dotnet restore ./src/IpGeoApi/IpGeoApi.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish ./src/IpGeoApi/IpGeoApi.csproj -c Release -o /app/publish

# ==============================
# Runtime Stage
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Expose port
EXPOSE 80

# Copy published output
COPY --from=build /app/publish .

# Run application
ENTRYPOINT ["dotnet", "IpGeoApi.dll"]