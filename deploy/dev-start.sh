#!/bin/bash

# School Medical Server - Development Deployment Script
# This script starts the application in development mode

set -e

echo "🏥 School Medical Server - Development Deployment"
echo "=================================================="

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker and try again."
    exit 1
fi

# Check if Docker Compose is available
if ! command -v docker-compose > /dev/null 2>&1; then
    echo "❌ Error: Docker Compose is not installed."
    exit 1
fi

# Create environment file if it doesn't exist
if [ ! -f .env.local ]; then
    echo "📝 Creating .env.local from example..."
    cp .env.example .env.local
    echo "✅ Created .env.local. Please edit it with your configuration."
fi

# Ask user for database preference
echo ""
echo "Choose database:"
echo "1) SQL Server (default)"
echo "2) PostgreSQL"
read -p "Enter choice (1 or 2): " db_choice

# Set up compose files based on choice
COMPOSE_FILES="-f docker-compose.yml -f docker-compose.dev.yml"

if [ "$db_choice" = "2" ]; then
    COMPOSE_FILES="$COMPOSE_FILES -f docker-compose.postgres.yml --profile postgres"
    echo "🐘 Using PostgreSQL database"
else
    echo "🗃️ Using SQL Server database"
fi

# Pull latest images
echo ""
echo "📥 Pulling latest Docker images..."
docker-compose $COMPOSE_FILES pull

# Start services
echo ""
echo "🚀 Starting services..."
docker-compose $COMPOSE_FILES up -d

# Wait for services to be healthy
echo ""
echo "⏳ Waiting for services to be ready..."
sleep 10

# Check health
echo ""
echo "🔍 Checking service health..."

if curl -f http://localhost:8080/health > /dev/null 2>&1; then
    echo "✅ API is healthy"
else
    echo "⚠️ API health check failed - check logs with: docker-compose logs school-medical-api"
fi

# Show service status
echo ""
echo "📊 Service Status:"
docker-compose $COMPOSE_FILES ps

echo ""
echo "🎉 Development environment is ready!"
echo ""
echo "🌐 Access URLs:"
echo "   API: http://localhost:8080"
echo "   Swagger: http://localhost:8080/swagger"
echo "   Health: http://localhost:8080/health"

if [ "$db_choice" = "2" ]; then
    echo "   Database: localhost:5432 (PostgreSQL)"
else
    echo "   Database: localhost:1433 (SQL Server)"
fi

echo ""
echo "📝 Useful commands:"
echo "   View logs: docker-compose logs -f"
echo "   Stop services: docker-compose down"
echo "   Restart API: docker-compose restart school-medical-api"
echo ""