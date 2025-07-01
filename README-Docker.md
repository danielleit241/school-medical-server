# Docker Deployment Guide for School Medical Server

This guide provides complete instructions for deploying the School Medical Server using Docker and Docker Compose.

## 📋 Prerequisites

- Docker Engine 20.10+ 
- Docker Compose 2.0+
- At least 4GB of available RAM
- 10GB of free disk space

## 🚀 Quick Start

### 1. Clone and Setup

```bash
git clone https://github.com/danielleit241/school-medical-server.git
cd school-medical-server
```

### 2. Configure Environment

Copy the example environment file and customize it:

```bash
cp .env.example .env.local
# Edit .env.local with your specific configuration
```

### 3. Start the Application

**For Development:**
```bash
# Using SQL Server (default)
docker-compose up -d

# Using PostgreSQL
docker-compose --profile postgres up -d
```

**For Production:**
```bash
# Copy production environment
cp .env.production .env.local
# Edit .env.local with your production settings

# Start with production configuration
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### 4. Access the Application

- **API**: http://localhost:8080
- **Swagger Documentation**: http://localhost:8080/swagger (Development only)
- **Health Check**: http://localhost:8080/health
- **Database**: localhost:1433 (SQL Server) or localhost:5432 (PostgreSQL)

## 🏗️ Architecture

### Multi-Stage Dockerfile

The Dockerfile implements a secure multi-stage build:

1. **Build Stage**: Uses `mcr.microsoft.com/dotnet/sdk:8.0` to build the application
2. **Publish Stage**: Publishes the application with optimizations
3. **Runtime Stage**: Uses `mcr.microsoft.com/dotnet/aspnet:8.0` for minimal runtime image

**Security Features:**
- Non-root user execution
- Minimal runtime image
- Health checks included
- Optimized layer caching

### Services

#### school-medical-api
- Main ASP.NET Core Web API application
- Exposes ports 8080 (HTTP)
- Includes health checks
- Automatic database migration and seeding

#### sqlserver
- Microsoft SQL Server 2022 Express
- Persistent data storage via Docker volumes
- Health checks for database availability
- Database initialization scripts

#### postgres (Optional)
- PostgreSQL 15 Alpine
- Alternative to SQL Server
- Lightweight and performant
- Use `--profile postgres` to enable

## ⚙️ Configuration

### Environment Variables

| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `ENVIRONMENT` | Application environment | Development | ✅ |
| `DB_PASSWORD` | Database password | YourStrong!Passw0rd | ✅ |
| `JWT_KEY` | JWT signing key | (generated) | ✅ |
| `JWT_ISSUER` | JWT issuer URL | http://localhost:8080 | ✅ |
| `ADMIN_PHONE` | Default admin phone | adminsystem | ✅ |
| `ADMIN_PASSWORD` | Default admin password | adminsystem | ✅ |
| `EMAIL_HOST` | SMTP server host | smtp.gmail.com | ❌ |
| `EMAIL_USERNAME` | Email username | - | ❌ |
| `EMAIL_PASSWORD` | Email password | - | ❌ |

### Environment Files

- **`.env.example`**: Template file with all available options
- **`.env.development`**: Development-specific settings
- **`.env.production`**: Production-ready configuration with security notes
- **`.env.local`**: Your local customizations (create from .env.example)

## 🛠️ Development

### Development Mode

Start with development overrides:

```bash
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up
```

Features:
- Source code volume mounting (for live reload)
- Exposed database ports
- Development logging
- Swagger UI enabled

### Using Different Databases

**SQL Server (Default):**
```bash
docker-compose up -d
```

**PostgreSQL:**
```bash
docker-compose --profile postgres up -d
```

**Switch Between Databases:**
```bash
# Stop current setup
docker-compose down

# Start with PostgreSQL
docker-compose -f docker-compose.yml -f docker-compose.postgres.yml up -d
```

## 🔒 Production Deployment

### Security Checklist

Before deploying to production:

- [ ] Change all default passwords
- [ ] Generate new JWT signing keys
- [ ] Configure proper email settings
- [ ] Set up SSL/TLS termination
- [ ] Configure firewall rules
- [ ] Set up backup strategies
- [ ] Configure log management

### Production Configuration

1. **Environment Setup:**
```bash
cp .env.production .env.local
# Edit .env.local with your production values
```

2. **Start Production Services:**
```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

3. **Production Features:**
- Resource limits (512MB RAM, 0.5 CPU for API)
- No exposed database ports
- Restart policies
- Log rotation
- Health checks

### SSL/TLS with Nginx (Optional)

Add a reverse proxy with SSL:

```yaml
# docker-compose.nginx.yml
version: '3.8'
services:
  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
      - ./ssl:/etc/nginx/ssl:ro
    depends_on:
      - school-medical-api
```

## 📊 Monitoring and Health Checks

### Health Endpoints

- **Application Health**: `GET /health`
- **Database Health**: Included in application health check

### Docker Health Checks

All services include health checks:
- API: HTTP health endpoint
- SQL Server: Connection test
- PostgreSQL: `pg_isready` check

### Monitoring Commands

```bash
# Check service status
docker-compose ps

# View logs
docker-compose logs -f school-medical-api
docker-compose logs -f sqlserver

# Check health
curl http://localhost:8080/health
```

## 🗃️ Data Management

### Database Persistence

Data is persisted using Docker volumes:
- `school-medical-sqlserver-data`: SQL Server data
- `school-medical-postgres-data`: PostgreSQL data

### Backup and Restore

**SQL Server Backup:**
```bash
# Create backup
docker exec school-medical-sqlserver sqlcmd -S localhost -U sa -P 'YourPassword' -Q "BACKUP DATABASE SchoolMedicalManagement TO DISK = '/var/backups/backup.bak'"

# Copy backup from container
docker cp school-medical-sqlserver:/var/backups/backup.bak ./backup.bak
```

**PostgreSQL Backup:**
```bash
# Create backup
docker exec school-medical-postgres pg_dump -U postgres SchoolMedicalManagement > backup.sql

# Restore backup
docker exec -i school-medical-postgres psql -U postgres SchoolMedicalManagement < backup.sql
```

## 🐛 Troubleshooting

### Common Issues

**1. Database Connection Failed**
```bash
# Check database is running
docker-compose ps sqlserver

# Check logs
docker-compose logs sqlserver

# Verify connection string in environment
```

**2. Application Won't Start**
```bash
# Check application logs
docker-compose logs school-medical-api

# Verify environment variables
docker-compose config

# Check health endpoint
curl http://localhost:8080/health
```

**3. Permission Denied Errors**
```bash
# Fix volume permissions
docker-compose down
docker volume rm school-medical-sqlserver-data
docker-compose up -d
```

### Useful Commands

```bash
# Rebuild application image
docker-compose build school-medical-api

# Reset database
docker-compose down -v
docker-compose up -d

# View real-time logs
docker-compose logs -f

# Execute commands in container
docker-compose exec school-medical-api bash
docker-compose exec sqlserver bash
```

## 🔧 Customization

### Adding Custom Configurations

1. **Create custom docker-compose override:**
```yaml
# docker-compose.override.yml
version: '3.8'
services:
  school-medical-api:
    environment:
      - CUSTOM_SETTING=value
    volumes:
      - ./custom-config:/app/config
```

2. **Extend the Dockerfile:**
```dockerfile
# Dockerfile.custom
FROM school-medical-server:latest
COPY custom-files/ /app/custom/
```

### Environment-Specific Deployments

Create additional compose files for specific environments:

```bash
# Staging
docker-compose -f docker-compose.yml -f docker-compose.staging.yml up -d

# Testing
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d
```

## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Reference](https://docs.docker.com/compose/)
- [ASP.NET Core in Docker](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/)
- [SQL Server Docker Documentation](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-deployment)

## 🤝 Contributing

When contributing Docker-related changes:

1. Test with both SQL Server and PostgreSQL
2. Verify both development and production configurations
3. Update documentation for new environment variables
4. Test health checks and monitoring
5. Validate security configurations

## 📄 License

This Docker deployment setup follows the same license as the main project.