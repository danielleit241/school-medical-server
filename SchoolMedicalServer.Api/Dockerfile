# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["SchoolMedicalServer.Api/SchoolMedicalServer.Api.csproj", "SchoolMedicalServer.Api/"]
COPY ["SchoolMedicalServer.Infrastructure/SchoolMedicalServer.Infrastructure.csproj", "SchoolMedicalServer.Infrastructure/"]
COPY ["SchoolMedicalServer.Abstractions/SchoolMedicalServer.Abstractions.csproj", "SchoolMedicalServer.Abstractions/"]

RUN dotnet restore "SchoolMedicalServer.Api/SchoolMedicalServer.Api.csproj"

# Copy source code and build
COPY . .
WORKDIR "/src/SchoolMedicalServer.Api"
RUN dotnet build "SchoolMedicalServer.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SchoolMedicalServer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

# Expose ports
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "SchoolMedicalServer.Api.dll"]