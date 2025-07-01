#!/bin/bash

# School Medical Server - Production Deployment Script
# This script deploys the application in production mode

set -e

echo "🏥 School Medical Server - Production Deployment"
echo "================================================"

# Check if running as root (recommended for production)
if [ "$EUID" -ne 0 ]; then
    echo "⚠️ Warning: Not running as root. Some operations may require sudo."
fi

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker and try again."
    exit 1
fi

# Check for production environment file
if [ ! -f .env.local ]; then
    echo "❌ Error: .env.local not found."
    echo "Please create .env.local based on .env.production and configure all settings."
    exit 1
fi

# Validate critical environment variables
echo "🔍 Validating production configuration..."

source .env.local

if [ -z "$DB_PASSWORD" ] || [ "$DB_PASSWORD" = "YourStrong!Passw0rd" ]; then
    echo "❌ Error: Please set a secure DB_PASSWORD in .env.local"
    exit 1
fi

if [ -z "$JWT_KEY" ] || [ "$JWT_KEY" = "3OtmdcjooTLneJsdUbNRSFLw92UiMSRoFoKnedu8vRSlqHkLcJ7okRBTQkh3SBLF" ]; then
    echo "❌ Error: Please set a secure JWT_KEY in .env.local"
    exit 1
fi

if [ -z "$ADMIN_PASSWORD" ] || [ "$ADMIN_PASSWORD" = "adminsystem" ]; then
    echo "❌ Error: Please set a secure ADMIN_PASSWORD in .env.local"
    exit 1
fi

echo "✅ Configuration validation passed"

# Create backup of existing deployment
if docker-compose ps | grep -q "school-medical"; then
    echo "💾 Creating backup of current deployment..."
    docker-compose exec sqlserver sqlcmd -S localhost -U sa -P "$DB_PASSWORD" -Q "BACKUP DATABASE SchoolMedicalManagement TO DISK = '/var/backups/pre-deploy-$(date +%Y%m%d_%H%M%S).bak'" || echo "⚠️ Backup failed or skipped"
fi

# Ask user for database preference
echo ""
echo "Choose database:"
echo "1) SQL Server (recommended for production)"
echo "2) PostgreSQL"
read -p "Enter choice (1 or 2): " db_choice

# Set up compose files
COMPOSE_FILES="-f docker-compose.yml -f docker-compose.prod.yml"

if [ "$db_choice" = "2" ]; then
    COMPOSE_FILES="$COMPOSE_FILES -f docker-compose.postgres.yml --profile postgres"
    echo "🐘 Using PostgreSQL database"
else
    echo "🗃️ Using SQL Server database"
fi

# Pull latest images
echo ""
echo "📥 Pulling latest production images..."
docker-compose $COMPOSE_FILES pull

# Build application image
echo ""
echo "🔨 Building application image..."
docker-compose $COMPOSE_FILES build --no-cache school-medical-api

# Stop existing services gracefully
if docker-compose ps | grep -q "school-medical"; then
    echo ""
    echo "⏹️ Stopping existing services..."
    docker-compose down --timeout 30
fi

# Start services
echo ""
echo "🚀 Starting production services..."
docker-compose $COMPOSE_FILES up -d

# Wait for services to be healthy
echo ""
echo "⏳ Waiting for services to be ready..."
sleep 30

# Health check with retries
echo ""
echo "🔍 Checking service health..."
for i in {1..10}; do
    if curl -f http://localhost:8080/health > /dev/null 2>&1; then
        echo "✅ API is healthy"
        break
    else
        if [ $i -eq 10 ]; then
            echo "❌ API health check failed after 10 attempts"
            echo "Check logs with: docker-compose logs school-medical-api"
            exit 1
        fi
        echo "⏳ Attempt $i/10 - waiting for API..."
        sleep 10
    fi
done

# Show service status
echo ""
echo "📊 Service Status:"
docker-compose $COMPOSE_FILES ps

# Show resource usage
echo ""
echo "📈 Resource Usage:"
docker stats --no-stream --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.MemPerc}}"

echo ""
echo "🎉 Production deployment completed successfully!"
echo ""
echo "🌐 Access URLs:"
echo "   API: http://localhost:8080"
echo "   Health: http://localhost:8080/health"

if [ "$db_choice" = "2" ]; then
    echo "   Database: Internal PostgreSQL (not exposed)"
else
    echo "   Database: Internal SQL Server (not exposed)"
fi

echo ""
echo "🔒 Security Reminders:"
echo "   - Set up SSL/TLS termination (nginx, load balancer, etc.)"
echo "   - Configure firewall rules"
echo "   - Set up log monitoring"
echo "   - Schedule regular backups"
echo "   - Monitor resource usage"
echo ""
echo "📝 Management commands:"
echo "   View logs: docker-compose logs -f"
echo "   Stop services: docker-compose down"
echo "   Restart API: docker-compose restart school-medical-api"
echo "   Scale API: docker-compose up -d --scale school-medical-api=2"
echo ""