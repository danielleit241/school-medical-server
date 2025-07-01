#!/bin/bash

# School Medical Server - Backup Script
# This script creates backups of the database and configuration

set -e

echo "🏥 School Medical Server - Backup Script"
echo "========================================"

# Check if services are running
if ! docker-compose ps | grep -q "school-medical"; then
    echo "❌ Error: School Medical Server services are not running."
    echo "Start services first with: ./deploy/dev-start.sh or ./deploy/prod-deploy.sh"
    exit 1
fi

# Create backup directory
BACKUP_DIR="backups/$(date +%Y-%m-%d)"
mkdir -p "$BACKUP_DIR"

echo "📁 Backup directory: $BACKUP_DIR"

# Load environment variables
if [ -f .env.local ]; then
    source .env.local
else
    echo "⚠️ Warning: .env.local not found, using defaults"
    DB_PASSWORD=${DB_PASSWORD:-"YourStrong!Passw0rd"}
fi

# Determine which database is running
if docker-compose ps | grep -q "postgres"; then
    DATABASE_TYPE="postgres"
elif docker-compose ps | grep -q "sqlserver"; then
    DATABASE_TYPE="sqlserver"
else
    echo "❌ Error: No supported database container found."
    exit 1
fi

echo "🗃️ Detected database: $DATABASE_TYPE"

# Create database backup
echo ""
echo "💾 Creating database backup..."

if [ "$DATABASE_TYPE" = "postgres" ]; then
    # PostgreSQL backup
    BACKUP_FILE="$BACKUP_DIR/database_postgres_$(date +%Y%m%d_%H%M%S).sql"
    docker-compose exec -T postgres pg_dump -U postgres SchoolMedicalManagement > "$BACKUP_FILE"
    
    if [ $? -eq 0 ]; then
        echo "✅ PostgreSQL backup created: $BACKUP_FILE"
        echo "📊 Backup size: $(du -h "$BACKUP_FILE" | cut -f1)"
    else
        echo "❌ PostgreSQL backup failed"
        exit 1
    fi
    
elif [ "$DATABASE_TYPE" = "sqlserver" ]; then
    # SQL Server backup
    BACKUP_FILE="database_sqlserver_$(date +%Y%m%d_%H%M%S).bak"
    
    # Create backup inside container
    docker-compose exec sqlserver sqlcmd -S localhost -U sa -P "$DB_PASSWORD" -Q "BACKUP DATABASE SchoolMedicalManagement TO DISK = '/tmp/$BACKUP_FILE' WITH COMPRESSION, CHECKSUM"
    
    if [ $? -eq 0 ]; then
        # Copy backup from container
        docker cp "school-medical-sqlserver:/tmp/$BACKUP_FILE" "$BACKUP_DIR/$BACKUP_FILE"
        docker-compose exec sqlserver rm "/tmp/$BACKUP_FILE"
        
        echo "✅ SQL Server backup created: $BACKUP_DIR/$BACKUP_FILE"
        echo "📊 Backup size: $(du -h "$BACKUP_DIR/$BACKUP_FILE" | cut -f1)"
    else
        echo "❌ SQL Server backup failed"
        exit 1
    fi
fi

# Backup configuration files
echo ""
echo "⚙️ Backing up configuration files..."

CONFIG_BACKUP="$BACKUP_DIR/config_$(date +%Y%m%d_%H%M%S).tar.gz"

tar -czf "$CONFIG_BACKUP" \
    .env.local \
    docker-compose.yml \
    docker-compose.*.yml \
    scripts/ \
    --exclude='.git' \
    2>/dev/null || echo "⚠️ Some config files may not exist"

if [ -f "$CONFIG_BACKUP" ]; then
    echo "✅ Configuration backup created: $CONFIG_BACKUP"
    echo "📊 Config backup size: $(du -h "$CONFIG_BACKUP" | cut -f1)"
fi

# Create backup metadata
METADATA_FILE="$BACKUP_DIR/backup_info.txt"
cat > "$METADATA_FILE" << EOF
School Medical Server Backup Information
========================================

Backup Date: $(date)
Database Type: $DATABASE_TYPE
Environment: ${ENVIRONMENT:-Development}

Database Backup: $(basename "$BACKUP_FILE" 2>/dev/null || echo "N/A")
Config Backup: $(basename "$CONFIG_BACKUP" 2>/dev/null || echo "N/A")

Docker Images:
$(docker-compose images --format "{{.Service}}: {{.Repository}}:{{.Tag}}")

Service Status at Backup:
$(docker-compose ps --format "{{.Service}}: {{.State}}")

System Information:
Host: $(hostname)
Docker Version: $(docker --version)
Compose Version: $(docker-compose --version)
EOF

echo "📝 Backup metadata: $METADATA_FILE"

# Calculate total backup size
TOTAL_SIZE=$(du -sh "$BACKUP_DIR" | cut -f1)

echo ""
echo "🎉 Backup completed successfully!"
echo "📊 Total backup size: $TOTAL_SIZE"
echo "📁 Backup location: $BACKUP_DIR"
echo ""
echo "🔍 Backup contents:"
ls -la "$BACKUP_DIR"

echo ""
echo "💡 Backup recommendations:"
echo "   - Store backups in a secure, off-site location"
echo "   - Test backup restoration regularly"
echo "   - Keep multiple backup generations"
echo "   - Consider automating backups with cron"
echo ""
echo "📋 To restore from this backup:"
if [ "$DATABASE_TYPE" = "postgres" ]; then
    echo "   docker-compose exec -T postgres psql -U postgres -c 'DROP DATABASE IF EXISTS SchoolMedicalManagement;'"
    echo "   docker-compose exec -T postgres psql -U postgres -c 'CREATE DATABASE SchoolMedicalManagement;'"
    echo "   docker-compose exec -T postgres psql -U postgres SchoolMedicalManagement < $BACKUP_FILE"
elif [ "$DATABASE_TYPE" = "sqlserver" ]; then
    echo "   docker cp $BACKUP_DIR/$BACKUP_FILE school-medical-sqlserver:/tmp/"
    echo "   docker-compose exec sqlserver sqlcmd -S localhost -U sa -P '$DB_PASSWORD' -Q \"RESTORE DATABASE SchoolMedicalManagement FROM DISK = '/tmp/$BACKUP_FILE' WITH REPLACE\""
fi
echo ""