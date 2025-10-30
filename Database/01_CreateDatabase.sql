-- =============================================
-- Script de Creación de Base de Datos
-- Sistema de Alquiler de Autos
-- =============================================

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SistemaAlquilerAutos')
BEGIN
    CREATE DATABASE SistemaAlquilerAutos;
END
GO

USE SistemaAlquilerAutos;
GO

PRINT 'Base de datos SistemaAlquilerAutos creada/seleccionada correctamente.';
GO
