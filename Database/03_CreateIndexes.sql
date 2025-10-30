-- =============================================
-- Script de Creación de Índices
-- Sistema de Alquiler de Autos
-- =============================================

USE SistemaAlquilerAutos;
GO

-- =============================================
-- Índices para Categorias
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Categorias_Nombre')
BEGIN
    CREATE INDEX IX_Categorias_Nombre ON Categorias(Nombre);
    PRINT 'Índice IX_Categorias_Nombre creado.';
END
GO

-- =============================================
-- Índices para Sucursales
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sucursales_Ciudad')
BEGIN
    CREATE INDEX IX_Sucursales_Ciudad ON Sucursales(Ciudad);
    PRINT 'Índice IX_Sucursales_Ciudad creado.';
END
GO

-- =============================================
-- Índices para Vehiculos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vehiculos_Estado')
BEGIN
    CREATE INDEX IX_Vehiculos_Estado ON Vehiculos(Estado);
    PRINT 'Índice IX_Vehiculos_Estado creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vehiculos_CategoriaId')
BEGIN
    CREATE INDEX IX_Vehiculos_CategoriaId ON Vehiculos(CategoriaId);
    PRINT 'Índice IX_Vehiculos_CategoriaId creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vehiculos_SucursalId')
BEGIN
    CREATE INDEX IX_Vehiculos_SucursalId ON Vehiculos(SucursalId);
    PRINT 'Índice IX_Vehiculos_SucursalId creado.';
END
GO

-- =============================================
-- Índices para Clientes
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Clientes_Apellido')
BEGIN
    CREATE INDEX IX_Clientes_Apellido ON Clientes(Apellido, Nombre);
    PRINT 'Índice IX_Clientes_Apellido creado.';
END
GO

-- =============================================
-- Índices para Alquileres
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Alquileres_ClienteId')
BEGIN
    CREATE INDEX IX_Alquileres_ClienteId ON Alquileres(ClienteId);
    PRINT 'Índice IX_Alquileres_ClienteId creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Alquileres_VehiculoId')
BEGIN
    CREATE INDEX IX_Alquileres_VehiculoId ON Alquileres(VehiculoId);
    PRINT 'Índice IX_Alquileres_VehiculoId creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Alquileres_Estado')
BEGIN
    CREATE INDEX IX_Alquileres_Estado ON Alquileres(Estado);
    PRINT 'Índice IX_Alquileres_Estado creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Alquileres_FechaInicio')
BEGIN
    CREATE INDEX IX_Alquileres_FechaInicio ON Alquileres(FechaInicio);
    PRINT 'Índice IX_Alquileres_FechaInicio creado.';
END
GO

PRINT 'Todos los índices han sido creados correctamente.';
GO
