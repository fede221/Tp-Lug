# Sistema de Alquiler de Autos

Sistema de gestión de alquiler de vehículos desarrollado con arquitectura en capas para C# .NET.

## Descripción del Sistema

Sistema integral para la gestión de alquiler de vehículos que permite:

- **Gestión de Clientes**: Alta, baja, modificación y consulta de clientes
- **Gestión de Vehículos**: Administración del parque automotor disponible
- **Gestión de Categorías**: Diferentes categorías de vehículos con precios diferenciados
- **Gestión de Sucursales**: Múltiples ubicaciones para retiro y devolución
- **Gestión de Alquileres**: Control completo del proceso de alquiler y devolución

## Arquitectura

El sistema está desarrollado con **arquitectura en capas**:

### 1. Capa de Entidades (Entity)
- **Proyecto**: `SistemaAlquilerAutos.Entity`
- **Responsabilidad**: Definición de las entidades del dominio
- **Entidades**:
  - `Cliente`: Información de clientes
  - `Vehiculo`: Datos de los vehículos
  - `Categoria`: Categorías de vehículos
  - `Sucursal`: Sucursales de la empresa
  - `Alquiler`: Registros de alquileres

### 2. Capa de Mapeo (Mapper)
- **Proyecto**: `SistemaAlquilerAutos.Mapper`
- **Responsabilidad**: Conversión de DataRow a objetos de negocio
- **Clases**:
  - `ClienteMapper`
  - `VehiculoMapper`
  - `CategoriaMapper`
  - `SucursalMapper`
  - `AlquilerMapper`

### 3. Capa de Acceso a Datos (DAL - Data Access Layer)
- **Proyecto**: `SistemaAlquilerAutos.DAL`
- **Responsabilidad**: Comunicación con la base de datos
- **Componentes**:
  - `DatabaseHelper`: Clase utilitaria para operaciones de BD
  - `TransactionManager`: Manejo de transacciones
  - Clases DAL por entidad (ClienteDAL, VehiculoDAL, etc.)
- **Características**:
  - Uso de SqlParameters para prevenir SQL Injection
  - Manejo de transacciones
  - Liberación correcta de recursos con `using`

### 4. Capa de Lógica de Negocio (BLL - Business Logic Layer)
- **Proyecto**: `SistemaAlquilerAutos.BLL`
- **Responsabilidad**: Implementación de reglas de negocio
- **Reglas de Negocio Implementadas**:
  - Validación de edad mínima para clientes (18 años)
  - Validación de formato de DNI y patentes
  - Verificación de duplicados (DNI, Patente)
  - Control de disponibilidad de vehículos
  - Cálculo automático de precios con descuentos por volumen:
    - 5% descuento: 7-14 días
    - 10% descuento: 15-29 días
    - 15% descuento: 30+ días
  - Control de alquileres activos
  - Validación de fechas y kilometrajes
- **Excepciones Personalizadas**:
  - `BusinessException`: Base para excepciones de negocio
  - `BusinessRuleException`: Violación de reglas de negocio
  - `EntityNotFoundException`: Entidad no encontrada
  - `DuplicateEntityException`: Entidad duplicada

### 5. Capa de Presentación (UI)
- **Proyecto**: `SistemaAlquilerAutos.UI`
- **Tecnología**: Windows Forms (.NET 6)
- **Formularios**:
  - `FormPrincipal`: Formulario principal con menú MDI
  - `FormClientes`: Gestión de clientes
  - `FormEditarCliente`: Alta/modificación de clientes
  - `FormVehiculos`: Gestión de vehículos
  - `FormCategorias`: Gestión de categorías
  - `FormSucursales`: Gestión de sucursales
  - `FormAlquileresActivos`: Visualización de alquileres activos
  - `FormHistorialAlquileres`: Historial completo
  - `FormNuevoAlquiler`: Creación de alquileres (en desarrollo)
  - `FormDevolverVehiculo`: Devolución de vehículos (en desarrollo)

## Base de Datos

### Diagrama Entidad-Relación

Ver archivo: [`Database/DiagramaER.md`](Database/DiagramaER.md)

### Tablas

1. **Categorias**
   - Id (PK)
   - Nombre, Descripcion, PrecioDiario
   - Activo

2. **Sucursales**
   - Id (PK)
   - Nombre, Direccion, Ciudad, Telefono, Email
   - Activo

3. **Vehiculos**
   - Id (PK)
   - Marca, Modelo, Anio, Patente (UNIQUE), Color, Kilometraje
   - Estado (Disponible/Alquilado/Mantenimiento/Inactivo)
   - CategoriaId (FK), SucursalId (FK)

4. **Clientes**
   - Id (PK)
   - DNI (UNIQUE), Nombre, Apellido, Email, Telefono
   - FechaNacimiento, Direccion, Ciudad
   - Activo

5. **Alquileres**
   - Id (PK)
   - ClienteId (FK), VehiculoId (FK)
   - FechaInicio, FechaFin, FechaDevolucionPrevista
   - SucursalRetiroId (FK), SucursalDevolucionId (FK)
   - KilometrajeInicio, KilometrajeFin
   - PrecioTotal, Estado, Observaciones

### Scripts SQL

Los scripts SQL están en la carpeta `Database/`:

1. `00_MasterScript.sql`: Script maestro que ejecuta todos los demás
2. `01_CreateDatabase.sql`: Creación de la base de datos
3. `02_CreateTables.sql`: Creación de tablas con constraints
4. `03_CreateIndexes.sql`: Creación de índices
5. `04_InsertInitialData.sql`: Datos iniciales de prueba

## Instalación y Configuración

### Requisitos Previos

- .NET 6 SDK o superior
- SQL Server 2019 o superior (o SQL Server Express)
- Visual Studio 2022 (recomendado) o Visual Studio Code

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <url-repositorio>
   cd Tp-Lug
   ```

2. **Crear la base de datos**
   - Abrir SQL Server Management Studio
   - Conectarse a la instancia de SQL Server
   - Ejecutar el script `Database/00_MasterScript.sql`
   - Alternativamente, ejecutar los scripts en orden (01, 02, 03, 04)

3. **Configurar la cadena de conexión**
   - Abrir el archivo `SistemaAlquilerAutos.UI/App.config`
   - Modificar la cadena de conexión según su configuración:
     ```xml
     <connectionStrings>
       <add name="SistemaAlquilerAutos"
            connectionString="Data Source=localhost;Initial Catalog=SistemaAlquilerAutos;Integrated Security=True;TrustServerCertificate=True"
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```
   - Para SQL Server Express: `Data Source=.\SQLEXPRESS;...`
   - Para autenticación SQL: Agregar `User ID=sa;Password=TuPassword;`

4. **Compilar la solución**
   ```bash
   dotnet build
   ```

5. **Ejecutar la aplicación**
   ```bash
   dotnet run --project SistemaAlquilerAutos.UI
   ```

## Uso del Sistema

### Inicio

Al iniciar el sistema, se presenta el formulario principal con un menú que permite acceder a todas las funcionalidades.

### Gestión de Clientes

1. Ir a **Gestión → Clientes**
2. Opciones disponibles:
   - **Buscar**: Buscar por nombre, apellido o DNI
   - **Nuevo Cliente**: Agregar un nuevo cliente
   - **Editar**: Modificar datos de un cliente existente
   - **Eliminar**: Dar de baja un cliente (soft delete)

### Gestión de Vehículos

1. Ir a **Gestión → Vehículos**
2. Visualizar todos los vehículos con su estado actual
3. Ver información de categoría y sucursal asignada

### Crear un Alquiler (En desarrollo - Etapa Inicial)

1. Ir a **Alquileres → Nuevo Alquiler**
2. Seleccionar cliente
3. Seleccionar vehículo disponible
4. Definir fechas de retiro y devolución
5. El sistema calculará automáticamente el precio con descuentos

### Consultar Alquileres

- **Alquileres Activos**: Ver todos los alquileres en curso
- **Historial**: Ver todos los alquileres (activos, completados, cancelados)

## Características Técnicas Destacadas

### Transacciones

Todas las operaciones que modifican múltiples tablas utilizan transacciones:
- Creación de alquiler: Inserta alquiler + actualiza estado del vehículo
- Finalización de alquiler: Actualiza alquiler + actualiza vehículo
- Cancelación de alquiler: Cancela alquiler + libera vehículo

Ejemplo de rollback automático en caso de error:
```csharp
using (var transaction = new TransactionManager())
{
    try
    {
        // Operaciones
        _alquilerDAL.Insert(alquiler, transaction);
        _vehiculoDAL.UpdateEstado(vehiculoId, EstadoAlquilado, transaction);

        transaction.Commit(); // Todo OK
    }
    catch
    {
        transaction.Rollback(); // Revertir en caso de error
        throw;
    }
}
```

### Validaciones

- **Nivel de Datos**: Constraints en base de datos
- **Nivel de Negocio**: Validaciones en capa BLL
- **Nivel de Presentación**: Validaciones en formularios

### Seguridad

- **SQL Injection**: Prevención mediante SqlParameters
- **Validación de Datos**: Todas las entradas son validadas
- **Manejo de Errores**: Try-catch en todas las capas

## Estado del Proyecto

**Etapa Actual**: Inicial

### Completado ✅

- ✅ Arquitectura en capas completa
- ✅ Base de datos con 5 tablas relacionadas
- ✅ Capa Entity con todas las entidades
- ✅ Capa Mapper completa
- ✅ Capa DAL con CRUD completo
- ✅ Capa BLL con reglas de negocio
- ✅ Manejo de transacciones
- ✅ Manejo de excepciones personalizadas
- ✅ Formularios principales de UI
- ✅ Gestión de Clientes (completa)
- ✅ Visualización de Categorías, Sucursales, Vehículos
- ✅ Visualización de Alquileres
- ✅ Scripts SQL completos
- ✅ Datos iniciales de prueba
- ✅ Archivo de configuración (App.config)

### En Desarrollo 🚧

- 🚧 Formularios de edición de Vehículos
- 🚧 Formularios de edición de Categorías
- 🚧 Formularios de edición de Sucursales
- 🚧 Formulario de Nuevo Alquiler
- 🚧 Formulario de Devolución de Vehículo

### Pendiente para Etapa Parcial 📋

- Integración completa con base de datos
- Pruebas exhaustivas de transacciones
- Reportes y estadísticas
- Validación de reglas de negocio adicionales
- Búsquedas avanzadas
- Filtros en listados

### Pendiente para Etapa Final 📋

- Manejo completo de transacciones de negocio
- Auditoría de operaciones
- Backup y restore
- Exportación de datos
- Manual de usuario completo
- Diagramas de clases
- Documentación técnica completa

## Tecnologías Utilizadas

- **Lenguaje**: C# 10
- **Framework**: .NET 6
- **UI**: Windows Forms
- **Base de Datos**: SQL Server 2019+
- **ADO.NET**: Para acceso a datos
- **Control de Versiones**: Git

## Estructura del Proyecto

```
Tp-Lug/
├── Database/
│   ├── 00_MasterScript.sql
│   ├── 01_CreateDatabase.sql
│   ├── 02_CreateTables.sql
│   ├── 03_CreateIndexes.sql
│   ├── 04_InsertInitialData.sql
│   └── DiagramaER.md
├── SistemaAlquilerAutos.Entity/
│   ├── Alquiler.cs
│   ├── Categoria.cs
│   ├── Cliente.cs
│   ├── Sucursal.cs
│   └── Vehiculo.cs
├── SistemaAlquilerAutos.Mapper/
│   ├── AlquilerMapper.cs
│   ├── CategoriaMapper.cs
│   ├── ClienteMapper.cs
│   ├── SucursalMapper.cs
│   └── VehiculoMapper.cs
├── SistemaAlquilerAutos.DAL/
│   ├── AlquilerDAL.cs
│   ├── CategoriaDAL.cs
│   ├── ClienteDAL.cs
│   ├── DatabaseHelper.cs
│   ├── SucursalDAL.cs
│   ├── TransactionManager.cs
│   └── VehiculoDAL.cs
├── SistemaAlquilerAutos.BLL/
│   ├── AlquilerBLL.cs
│   ├── CategoriaBLL.cs
│   ├── ClienteBLL.cs
│   ├── SucursalBLL.cs
│   ├── VehiculoBLL.cs
│   └── Exceptions/
│       └── BusinessException.cs
├── SistemaAlquilerAutos.UI/
│   ├── Program.cs
│   ├── FormPrincipal.cs
│   ├── App.config
│   └── Forms/
│       ├── FormClientes.cs
│       ├── FormEditarCliente.cs
│       ├── FormVehiculos.cs
│       ├── FormCategorias.cs
│       ├── FormSucursales.cs
│       ├── FormNuevoAlquiler.cs
│       ├── FormAlquileresActivos.cs
│       ├── FormHistorialAlquileres.cs
│       └── FormDevolverVehiculo.cs
├── SistemaAlquilerAutos.sln
└── README.md
```

## Autor

Trabajo Integrador - Laboratorio de Programación

## Licencia

Este proyecto es con fines educativos.
