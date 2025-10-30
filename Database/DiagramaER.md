# Diagrama Entidad-Relación
## Sistema de Alquiler de Autos

```
┌─────────────────────┐
│     Categorias      │
├─────────────────────┤
│ PK Id               │
│    Nombre           │
│    Descripcion      │
│    PrecioDiario     │
│    Activo           │
└──────────┬──────────┘
           │
           │ 1
           │
           │
           │ N
┌──────────┴──────────┐          ┌─────────────────────┐
│     Vehiculos       │          │     Sucursales      │
├─────────────────────┤          ├─────────────────────┤
│ PK Id               │          │ PK Id               │
│    Marca            │          │    Nombre           │
│    Modelo           │          │    Direccion        │
│    Anio             │          │    Ciudad           │
│    Patente (UNIQUE) │          │    Telefono         │
│    Color            │          │    Email            │
│    Kilometraje      │          │    Activo           │
│    Estado           │          └──────────┬──────────┘
│ FK CategoriaId      │                     │
│ FK SucursalId       │◄────────────────────┘
└──────────┬──────────┘                     1│N
           │                                  │
           │ N                                │
           │                                  │
           │ 1                                │
           │                          ┌───────┴──────────┐
┌──────────┴──────────┐               │                  │
│     Alquileres      │               │                  │
├─────────────────────┤               │ 1                │ 1
│ PK Id               │               │                  │
│ FK ClienteId        │               │                  │
│ FK VehiculoId       │               │                  │
│    FechaInicio      │               │                  │
│    FechaFin         │               │                  │
│    FechaDevPrev     │               │                  │
│ FK SucRetiroId      │───────────────┘                  │
│ FK SucDevolucionId  │──────────────────────────────────┘
│    KmInicio         │
│    KmFin            │
│    PrecioTotal      │
│    Estado           │
│    Observaciones    │
└──────────┬──────────┘
           │
           │ N
           │
           │ 1
┌──────────┴──────────┐
│      Clientes       │
├─────────────────────┤
│ PK Id               │
│    DNI (UNIQUE)     │
│    Nombre           │
│    Apellido         │
│    Email            │
│    Telefono         │
│    FechaNacimiento  │
│    Direccion        │
│    Ciudad           │
│    Activo           │
└─────────────────────┘
```

## Relaciones

### 1. Categorias - Vehiculos (1:N)
- Una categoría puede tener muchos vehículos
- Un vehículo pertenece a una sola categoría

### 2. Sucursales - Vehiculos (1:N)
- Una sucursal puede tener muchos vehículos
- Un vehículo está asignado a una sucursal

### 3. Clientes - Alquileres (1:N)
- Un cliente puede tener muchos alquileres
- Un alquiler pertenece a un solo cliente

### 4. Vehiculos - Alquileres (1:N)
- Un vehículo puede tener muchos alquileres
- Un alquiler tiene un solo vehículo

### 5. Sucursales - Alquileres (1:N) - Retiro
- Una sucursal puede ser punto de retiro de muchos alquileres
- Un alquiler tiene una sucursal de retiro

### 6. Sucursales - Alquileres (1:N) - Devolución
- Una sucursal puede ser punto de devolución de muchos alquileres
- Un alquiler puede tener una sucursal de devolución (opcional)

## Tipos de Datos y Constraints

### Categorias
- `PrecioDiario`: DECIMAL(10,2), CHECK > 0
- `Activo`: BIT, DEFAULT 1

### Vehiculos
- `Patente`: NVARCHAR(10), UNIQUE
- `Kilometraje`: INT, CHECK >= 0
- `Estado`: INT, CHECK IN (1,2,3,4)
  - 1: Disponible
  - 2: Alquilado
  - 3: Mantenimiento
  - 4: Inactivo

### Clientes
- `DNI`: NVARCHAR(8), UNIQUE
- `FechaNacimiento`: DATE, CHECK < GETDATE()

### Alquileres
- `Estado`: INT, CHECK IN (1,2,3)
  - 1: Activo
  - 2: Completado
  - 3: Cancelado
- `PrecioTotal`: DECIMAL(10,2), CHECK > 0
- Constraint: FechaDevolucionPrevista > FechaInicio

## Índices

- **Categorias**: Nombre
- **Sucursales**: Ciudad
- **Vehiculos**: Estado, CategoriaId, SucursalId
- **Clientes**: Apellido + Nombre
- **Alquileres**: ClienteId, VehiculoId, Estado, FechaInicio
