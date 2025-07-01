-- PostgreSQL Database Initialization Script
-- This script is executed when the PostgreSQL container starts for the first time

-- The database is already created by the POSTGRES_DB environment variable
-- This script can be used for additional setup if needed

-- Enable extensions that might be useful
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Set timezone
SET timezone = 'UTC';

-- Create a custom role for the application (optional)
-- DO $$ 
-- BEGIN
--     IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'schoolmedical_app') THEN
--         CREATE ROLE schoolmedical_app LOGIN PASSWORD 'app_password';
--         GRANT CONNECT ON DATABASE SchoolMedicalManagement TO schoolmedical_app;
--     END IF;
-- END
-- $$;

SELECT 'PostgreSQL database initialization completed' AS status;