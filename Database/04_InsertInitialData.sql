-- =============================================
-- Script de Inserción de Datos Iniciales
-- Sistema de Alquiler de Autos
-- =============================================

USE SistemaAlquilerAutos;
GO

-- =============================================
-- Insertar Categorías
-- =============================================
IF NOT EXISTS (SELECT * FROM Categorias)
BEGIN
    INSERT INTO Categorias (Nombre, Descripcion, PrecioDiario, Activo) VALUES
    ('Económico', 'Vehículos pequeños y económicos, ideales para ciudad', 5000.00, 1),
    ('Intermedio', 'Vehículos medianos con buen espacio y confort', 8000.00, 1),
    ('SUV', 'Vehículos utilitarios deportivos con mayor espacio', 12000.00, 1),
    ('Lujo', 'Vehículos de alta gama con máximo confort', 20000.00, 1),
    ('Familiar', 'Vehículos espaciosos para familias numerosas', 10000.00, 1);

    PRINT 'Categorías insertadas correctamente.';
END
GO

-- =============================================
-- Insertar Sucursales
-- =============================================
IF NOT EXISTS (SELECT * FROM Sucursales)
BEGIN
    INSERT INTO Sucursales (Nombre, Direccion, Ciudad, Telefono, Email, Activo) VALUES
    ('Sucursal Centro Córdoba', 'Av. Colón 500', 'Córdoba', '0351-4567890', 'centro.cordoba@alquilerautos.com', 1),
    ('Sucursal Nueva Córdoba', 'Av. Hipólito Yrigoyen 250', 'Córdoba', '0351-4567891', 'nuevacordoba@alquilerautos.com', 1),
    ('Sucursal Buenos Aires', 'Av. Corrientes 1500', 'Buenos Aires', '011-4567890', 'buenosaires@alquilerautos.com', 1),
    ('Sucursal Rosario', 'Bv. Oroño 800', 'Rosario', '0341-4567890', 'rosario@alquilerautos.com', 1),
    ('Sucursal Mendoza', 'Av. San Martín 1200', 'Mendoza', '0261-4567890', 'mendoza@alquilerautos.com', 1);

    PRINT 'Sucursales insertadas correctamente.';
END
GO

-- =============================================
-- Insertar Vehículos
-- =============================================
IF NOT EXISTS (SELECT * FROM Vehiculos)
BEGIN
    DECLARE @CategoriaEconomico INT = (SELECT Id FROM Categorias WHERE Nombre = 'Económico');
    DECLARE @CategoriaIntermedio INT = (SELECT Id FROM Categorias WHERE Nombre = 'Intermedio');
    DECLARE @CategoriaSUV INT = (SELECT Id FROM Categorias WHERE Nombre = 'SUV');
    DECLARE @CategoriaLujo INT = (SELECT Id FROM Categorias WHERE Nombre = 'Lujo');
    DECLARE @CategoriaFamiliar INT = (SELECT Id FROM Categorias WHERE Nombre = 'Familiar');

    DECLARE @SucursalCentroCba INT = (SELECT Id FROM Sucursales WHERE Nombre = 'Sucursal Centro Córdoba');
    DECLARE @SucursalNuevaCba INT = (SELECT Id FROM Sucursales WHERE Nombre = 'Sucursal Nueva Córdoba');
    DECLARE @SucursalBsAs INT = (SELECT Id FROM Sucursales WHERE Nombre = 'Sucursal Buenos Aires');
    DECLARE @SucursalRosario INT = (SELECT Id FROM Sucursales WHERE Nombre = 'Sucursal Rosario');

    INSERT INTO Vehiculos (Marca, Modelo, Anio, Patente, Color, Kilometraje, Estado, CategoriaId, SucursalId) VALUES
    -- Económicos
    ('Chevrolet', 'Onix', 2023, 'AA123BB', 'Blanco', 15000, 1, @CategoriaEconomico, @SucursalCentroCba),
    ('Fiat', 'Cronos', 2022, 'AB234CD', 'Gris', 28000, 1, @CategoriaEconomico, @SucursalCentroCba),
    ('Renault', 'Logan', 2023, 'AC345DE', 'Rojo', 12000, 1, @CategoriaEconomico, @SucursalNuevaCba),
    ('Volkswagen', 'Gol', 2021, 'AD456EF', 'Negro', 45000, 1, @CategoriaEconomico, @SucursalBsAs),

    -- Intermedios
    ('Toyota', 'Corolla', 2023, 'AE567FG', 'Plateado', 8000, 1, @CategoriaIntermedio, @SucursalCentroCba),
    ('Honda', 'Civic', 2022, 'AF678GH', 'Azul', 22000, 1, @CategoriaIntermedio, @SucursalNuevaCba),
    ('Chevrolet', 'Cruze', 2023, 'AG789HI', 'Blanco', 5000, 1, @CategoriaIntermedio, @SucursalBsAs),
    ('Peugeot', '408', 2022, 'AH890IJ', 'Gris', 18000, 1, @CategoriaIntermedio, @SucursalRosario),

    -- SUVs
    ('Toyota', 'RAV4', 2023, 'AI901JK', 'Negro', 10000, 1, @CategoriaSUV, @SucursalCentroCba),
    ('Jeep', 'Compass', 2022, 'AJ012KL', 'Blanco', 25000, 1, @CategoriaSUV, @SucursalNuevaCba),
    ('Chevrolet', 'Tracker', 2023, 'AK123LM', 'Gris', 7000, 1, @CategoriaSUV, @SucursalBsAs),
    ('Volkswagen', 'Tiguan', 2022, 'AL234MN', 'Azul', 30000, 1, @CategoriaSUV, @SucursalRosario),

    -- Lujo
    ('Audi', 'A4', 2023, 'AM345NO', 'Negro', 3000, 1, @CategoriaLujo, @SucursalCentroCba),
    ('BMW', '320i', 2023, 'AN456OP', 'Blanco', 2000, 1, @CategoriaLujo, @SucursalBsAs),
    ('Mercedes Benz', 'Clase C', 2022, 'AO567PQ', 'Plateado', 15000, 1, @CategoriaLujo, @SucursalBsAs),

    -- Familiares
    ('Peugeot', 'Partner', 2022, 'AP678QR', 'Blanco', 35000, 1, @CategoriaFamiliar, @SucursalCentroCba),
    ('Citroën', 'Berlingo', 2023, 'AQ789RS', 'Gris', 8000, 1, @CategoriaFamiliar, @SucursalNuevaCba),
    ('Renault', 'Kangoo', 2021, 'AR890ST', 'Rojo', 50000, 1, @CategoriaFamiliar, @SucursalRosario);

    PRINT 'Vehículos insertados correctamente.';
END
GO

-- =============================================
-- Insertar Clientes de Prueba
-- =============================================
IF NOT EXISTS (SELECT * FROM Clientes)
BEGIN
    INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono, FechaNacimiento, Direccion, Ciudad, Activo) VALUES
    ('12345678', 'Juan', 'Pérez', 'juan.perez@email.com', '351-5551234', '1990-05-15', 'Av. General Paz 123', 'Córdoba', 1),
    ('23456789', 'María', 'González', 'maria.gonzalez@email.com', '351-5555678', '1985-08-22', 'Calle San Martín 456', 'Córdoba', 1),
    ('34567890', 'Carlos', 'Rodríguez', 'carlos.rodriguez@email.com', '351-5559012', '1992-11-30', 'Av. Colón 789', 'Córdoba', 1),
    ('45678901', 'Ana', 'Martínez', 'ana.martinez@email.com', '011-5551234', '1988-03-10', 'Av. Santa Fe 1500', 'Buenos Aires', 1),
    ('56789012', 'Luis', 'Fernández', 'luis.fernandez@email.com', '0341-5555678', '1995-07-18', 'Bv. Oroño 2000', 'Rosario', 1);

    PRINT 'Clientes de prueba insertados correctamente.';
END
GO

PRINT 'Datos iniciales insertados correctamente.';
GO
