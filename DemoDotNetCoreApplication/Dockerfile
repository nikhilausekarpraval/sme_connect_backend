# Use ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 9090
EXPOSE 9091

# Use .NET SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["DemoDotNetCoreApplication.csproj", "."]
RUN dotnet restore "./DemoDotNetCoreApplication.csproj"

# Copy the rest of the files and build the application
COPY . .
RUN dotnet build "./DemoDotNetCoreApplication.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DemoDotNetCoreApplication.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoDotNetCoreApplication.dll"]
