-- =============================================
-- Script de Creación de Tablas
-- Sistema de Alquiler de Autos
-- =============================================

USE SistemaAlquilerAutos;
GO

-- =============================================
-- Tabla: Categorias
-- Descripción: Categorías de vehículos (Económico, SUV, etc.)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Categorias] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [PrecioDiario] DECIMAL(10, 2) NOT NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [CK_Categorias_PrecioDiario] CHECK ([PrecioDiario] > 0)
    );
    PRINT 'Tabla Categorias creada correctamente.';
END
GO

-- =============================================
-- Tabla: Sucursales
-- Descripción: Sucursales donde se pueden alquilar/devolver vehículos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sucursales]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sucursales] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [Direccion] NVARCHAR(200) NOT NULL,
        [Ciudad] NVARCHAR(100) NOT NULL,
        [Telefono] NVARCHAR(20) NOT NULL,
        [Email] NVARCHAR(100) NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Sucursales creada correctamente.';
END
GO

-- =============================================
-- Tabla: Vehiculos
-- Descripción: Vehículos disponibles para alquiler
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehiculos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehiculos] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [Marca] NVARCHAR(50) NOT NULL,
        [Modelo] NVARCHAR(50) NOT NULL,
        [Anio] INT NOT NULL,
        [Patente] NVARCHAR(10) NOT NULL UNIQUE,
        [Color] NVARCHAR(30) NOT NULL,
        [Kilometraje] INT NOT NULL DEFAULT 0,
        [Estado] INT NOT NULL DEFAULT 1, -- 1: Disponible, 2: Alquilado, 3: Mantenimiento, 4: Inactivo
        [CategoriaId] INT NOT NULL,
        [SucursalId] INT NOT NULL,
        CONSTRAINT [FK_Vehiculos_Categorias] FOREIGN KEY ([CategoriaId])
            REFERENCES [dbo].[Categorias]([Id]),
        CONSTRAINT [FK_Vehiculos_Sucursales] FOREIGN KEY ([SucursalId])
            REFERENCES [dbo].[Sucursales]([Id]),
        CONSTRAINT [CK_Vehiculos_Kilometraje] CHECK ([Kilometraje] >= 0),
        CONSTRAINT [CK_Vehiculos_Anio] CHECK ([Anio] >= 1900 AND [Anio] <= YEAR(GETDATE()) + 1),
        CONSTRAINT [CK_Vehiculos_Estado] CHECK ([Estado] IN (1, 2, 3, 4))
    );
    PRINT 'Tabla Vehiculos creada correctamente.';
END
GO

-- =============================================
-- Tabla: Clientes
-- Descripción: Clientes que alquilan vehículos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clientes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Clientes] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [DNI] NVARCHAR(8) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Apellido] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NULL,
        [Telefono] NVARCHAR(20) NOT NULL,
        [FechaNacimiento] DATE NOT NULL,
        [Direccion] NVARCHAR(200) NOT NULL,
        [Ciudad] NVARCHAR(100) NOT NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [CK_Clientes_FechaNacimiento] CHECK ([FechaNacimiento] < GETDATE())
    );
    PRINT 'Tabla Clientes creada correctamente.';
END
GO

-- =============================================
-- Tabla: Alquileres
-- Descripción: Registros de alquileres de vehículos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Alquileres]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Alquileres] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [ClienteId] INT NOT NULL,
        [VehiculoId] INT NOT NULL,
        [FechaInicio] DATETIME NOT NULL,
        [FechaFin] DATETIME NULL,
        [FechaDevolucionPrevista] DATETIME NOT NULL,
        [SucursalRetiroId] INT NOT NULL,
        [SucursalDevolucionId] INT NULL,
        [KilometrajeInicio] INT NOT NULL,
        [KilometrajeFin] INT NULL,
        [PrecioTotal] DECIMAL(10, 2) NOT NULL,
        [Estado] INT NOT NULL DEFAULT 1, -- 1: Activo, 2: Completado, 3: Cancelado
        [Observaciones] NVARCHAR(500) NULL,
        CONSTRAINT [FK_Alquileres_Clientes] FOREIGN KEY ([ClienteId])
            REFERENCES [dbo].[Clientes]([Id]),
        CONSTRAINT [FK_Alquileres_Vehiculos] FOREIGN KEY ([VehiculoId])
            REFERENCES [dbo].[Vehiculos]([Id]),
        CONSTRAINT [FK_Alquileres_SucursalRetiro] FOREIGN KEY ([SucursalRetiroId])
            REFERENCES [dbo].[Sucursales]([Id]),
        CONSTRAINT [FK_Alquileres_SucursalDevolucion] FOREIGN KEY ([SucursalDevolucionId])
            REFERENCES [dbo].[Sucursales]([Id]),
        CONSTRAINT [CK_Alquileres_FechaDevolucion] CHECK ([FechaDevolucionPrevista] > [FechaInicio]),
        CONSTRAINT [CK_Alquileres_PrecioTotal] CHECK ([PrecioTotal] > 0),
        CONSTRAINT [CK_Alquileres_Estado] CHECK ([Estado] IN (1, 2, 3))
    );
    PRINT 'Tabla Alquileres creada correctamente.';
END
GO

PRINT 'Todas las tablas han sido creadas correctamente.';
GO
