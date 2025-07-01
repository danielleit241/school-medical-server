# School Medical Server - Docker Deployment Makefile
# Use this Makefile to easily manage Docker deployments

.PHONY: help dev prod test stop clean logs health backup restore build

# Default target
help: ## Show this help message
	@echo "🏥 School Medical Server - Docker Deployment"
	@echo "============================================="
	@echo ""
	@echo "Available commands:"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)
	@echo ""
	@echo "Examples:"
	@echo "  make dev              # Start development environment"
	@echo "  make prod             # Deploy to production"
	@echo "  make logs service=api # View API logs"
	@echo "  make backup           # Create database backup"

# Development environment
dev: ## Start development environment with SQL Server
	@echo "🚀 Starting development environment..."
	@if [ ! -f .env.local ]; then cp .env.example .env.local; echo "📝 Created .env.local from example"; fi
	docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
	@make _wait-for-health
	@make _show-dev-info

dev-postgres: ## Start development environment with PostgreSQL
	@echo "🚀 Starting development environment with PostgreSQL..."
	@if [ ! -f .env.local ]; then cp .env.example .env.local; echo "📝 Created .env.local from example"; fi
	docker-compose -f docker-compose.yml -f docker-compose.dev.yml -f docker-compose.postgres.yml --profile postgres up -d
	@make _wait-for-health
	@make _show-dev-info

# Production environment
prod: ## Deploy to production
	@echo "🏭 Deploying to production..."
	@if [ ! -f .env.local ]; then echo "❌ .env.local not found. Copy from .env.production and configure."; exit 1; fi
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build
	@make _wait-for-health
	@make _show-prod-info

prod-postgres: ## Deploy to production with PostgreSQL
	@echo "🏭 Deploying to production with PostgreSQL..."
	@if [ ! -f .env.local ]; then echo "❌ .env.local not found. Copy from .env.production and configure."; exit 1; fi
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml -f docker-compose.postgres.yml --profile postgres up -d --build
	@make _wait-for-health
	@make _show-prod-info

# Testing
test: ## Run tests in containerized environment
	@echo "🧪 Running tests..."
	docker-compose -f docker-compose.yml run --rm school-medical-api dotnet test /src/SchoolMedicalServer.Test

build: ## Build the application image
	@echo "🔨 Building application image..."
	docker-compose build school-medical-api

# Operational commands
stop: ## Stop all services
	@echo "⏹️ Stopping all services..."
	docker-compose down

clean: ## Stop services and remove volumes (⚠️ deletes data)
	@echo "🧹 Cleaning up (this will delete data)..."
	@read -p "Are you sure? This will delete all data! (y/N): " confirm && [ "$$confirm" = "y" ]
	docker-compose down -v --remove-orphans
	docker system prune -f

restart: ## Restart all services
	@echo "🔄 Restarting services..."
	docker-compose restart

restart-api: ## Restart only the API service
	@echo "🔄 Restarting API service..."
	docker-compose restart school-medical-api

# Monitoring
logs: ## View logs (use service=<name> for specific service)
	@if [ -n "$(service)" ]; then \
		echo "📋 Viewing logs for $(service)..."; \
		docker-compose logs -f school-medical-$(service); \
	else \
		echo "📋 Viewing all logs..."; \
		docker-compose logs -f; \
	fi

status: ## Show service status
	@echo "📊 Service Status:"
	@docker-compose ps
	@echo ""
	@echo "📈 Resource Usage:"
	@docker stats --no-stream --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.MemPerc}}" | grep school-medical || echo "No services running"

health: ## Check application health
	@echo "🔍 Checking health..."
	@if curl -f http://localhost:8080/health > /dev/null 2>&1; then \
		echo "✅ API is healthy"; \
	else \
		echo "❌ API health check failed"; \
	fi

# Database operations
backup: ## Create database and configuration backup
	@echo "💾 Creating backup..."
	@./deploy/backup.sh

restore: ## Restore from backup (interactive)
	@echo "🔄 Restore from backup..."
	@echo "Available backups:"
	@ls -la backups/ 2>/dev/null || echo "No backups found"
	@read -p "Enter backup directory name (YYYY-MM-DD): " backup_date; \
	if [ -d "backups/$$backup_date" ]; then \
		echo "Restoring from backups/$$backup_date"; \
		echo "⚠️ This will replace current data!"; \
		read -p "Continue? (y/N): " confirm; \
		if [ "$$confirm" = "y" ]; then \
			make _restore-backup backup=$$backup_date; \
		fi; \
	else \
		echo "❌ Backup directory not found"; \
	fi

# Database access
db-sql: ## Connect to SQL Server database
	@echo "🗃️ Connecting to SQL Server..."
	docker-compose exec sqlserver sqlcmd -S localhost -U sa

db-psql: ## Connect to PostgreSQL database
	@echo "🐘 Connecting to PostgreSQL..."
	docker-compose exec postgres psql -U postgres SchoolMedicalManagement

# Development helpers
shell: ## Open shell in API container
	@echo "🐚 Opening shell in API container..."
	docker-compose exec school-medical-api bash

shell-db: ## Open shell in database container
	@if docker-compose ps | grep -q postgres; then \
		echo "🐚 Opening shell in PostgreSQL container..."; \
		docker-compose exec postgres bash; \
	elif docker-compose ps | grep -q sqlserver; then \
		echo "🐚 Opening shell in SQL Server container..."; \
		docker-compose exec sqlserver bash; \
	else \
		echo "❌ No database container found"; \
	fi

# Environment setup
setup-dev: ## Setup development environment from scratch
	@echo "🔧 Setting up development environment..."
	@cp .env.development .env.local
	@echo "✅ Created .env.local for development"
	@echo "📝 You can now run 'make dev' to start the environment"

setup-prod: ## Setup production environment template
	@echo "🔧 Setting up production environment template..."
	@cp .env.production .env.local
	@echo "✅ Created .env.local for production"
	@echo "⚠️ IMPORTANT: Edit .env.local with your production values before running 'make prod'"

# Internal helpers (don't call directly)
_wait-for-health:
	@echo "⏳ Waiting for services to be ready..."
	@for i in $$(seq 1 30); do \
		if curl -f http://localhost:8080/health > /dev/null 2>&1; then \
			echo "✅ Services are ready"; \
			break; \
		fi; \
		if [ $$i -eq 30 ]; then \
			echo "❌ Services failed to start"; \
			exit 1; \
		fi; \
		echo "⏳ Attempt $$i/30..."; \
		sleep 2; \
	done

_show-dev-info:
	@echo ""
	@echo "🎉 Development environment is ready!"
	@echo ""
	@echo "🌐 Access URLs:"
	@echo "   API: http://localhost:8080"
	@echo "   Swagger: http://localhost:8080/swagger"
	@echo "   Health: http://localhost:8080/health"
	@echo ""
	@echo "📝 Useful commands:"
	@echo "   make logs           # View all logs"
	@echo "   make logs service=api  # View API logs"
	@echo "   make status         # Check service status"
	@echo "   make stop           # Stop all services"

_show-prod-info:
	@echo ""
	@echo "🎉 Production deployment completed!"
	@echo ""
	@echo "🌐 Access URLs:"
	@echo "   API: http://localhost:8080"
	@echo "   Health: http://localhost:8080/health"
	@echo ""
	@echo "🔒 Next steps:"
	@echo "   - Configure SSL/TLS termination"
	@echo "   - Set up monitoring and alerting"
	@echo "   - Schedule regular backups"

_restore-backup:
	@echo "🔄 Restoring backup from $(backup)..."
	# Implementation depends on backup format and database type
	@echo "⚠️ Manual restoration required - check backup directory for instructions"