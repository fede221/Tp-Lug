-- =============================================
-- Script Maestro - Sistema de Alquiler de Autos
-- Ejecuta todos los scripts de creación en orden
-- =============================================

PRINT '===============================================';
PRINT 'Iniciando creación de Base de Datos';
PRINT 'Sistema de Alquiler de Autos';
PRINT '===============================================';
PRINT '';

-- Ejecutar scripts en orden
PRINT '1. Creando base de datos...';
:r 01_CreateDatabase.sql
PRINT '';

PRINT '2. Creando tablas...';
:r 02_CreateTables.sql
PRINT '';

PRINT '3. Creando índices...';
:r 03_CreateIndexes.sql
PRINT '';

PRINT '4. Insertando datos iniciales...';
:r 04_InsertInitialData.sql
PRINT '';

PRINT '===============================================';
PRINT 'Base de datos creada exitosamente';
PRINT '===============================================';
GO
