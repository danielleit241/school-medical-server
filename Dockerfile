# Multi-stage build for School Medical Server
# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file and project files
COPY school-medical-server.sln ./
COPY SchoolMedicalServer.Api/SchoolMedicalServer.Api.csproj SchoolMedicalServer.Api/
COPY SchoolMedicalServer.Abstractions/SchoolMedicalServer.Abstractions.csproj SchoolMedicalServer.Abstractions/
COPY SchoolMedicalServer.Infrastructure/SchoolMedicalServer.Infrastructure.csproj SchoolMedicalServer.Infrastructure/
COPY SchoolMedicalServer.Test/SchoolMedicalServer.Test.csproj SchoolMedicalServer.Test/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "SchoolMedicalServer.Api/SchoolMedicalServer.Api.csproj" -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish "SchoolMedicalServer.Api/SchoolMedicalServer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Set ownership and permissions
RUN chown -R appuser:appuser /app
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Configure environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check using dotnet
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet --info > /dev/null || exit 1

# Entry point
ENTRYPOINT ["dotnet", "SchoolMedicalServer.Api.dll"]