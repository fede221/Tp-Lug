using System.Data;
using System.Data.SqlClient;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.Mapper;

namespace SistemaAlquilerAutos.DAL
{
    public class VehiculoDAL
    {
        public List<Vehiculo> GetAll()
        {
            string query = @"
                SELECT v.Id, v.Marca, v.Modelo, v.Anio, v.Patente, v.Color,
                       v.Kilometraje, v.Estado, v.CategoriaId, v.SucursalId,
                       c.Nombre AS CategoriaNombre, c.PrecioDiario AS CategoriaPrecioDiario,
                       s.Nombre AS SucursalNombre, s.Ciudad AS SucursalCiudad
                FROM Vehiculos v
                INNER JOIN Categorias c ON v.CategoriaId = c.Id
                INNER JOIN Sucursales s ON v.SucursalId = s.Id
                WHERE v.Estado != @EstadoInactivo
                ORDER BY v.Marca, v.Modelo";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@EstadoInactivo", Vehiculo.ESTADO_INACTIVO)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return VehiculoMapper.MapList(dataTable);
        }

        public List<Vehiculo> GetDisponibles(int? sucursalId = null)
        {
            string query = sucursalId.HasValue
                ? @"SELECT v.Id, v.Marca, v.Modelo, v.Anio, v.Patente, v.Color,
                           v.Kilometraje, v.Estado, v.CategoriaId, v.SucursalId,
                           c.Nombre AS CategoriaNombre, c.PrecioDiario AS CategoriaPrecioDiario,
                           s.Nombre AS SucursalNombre, s.Ciudad AS SucursalCiudad
                    FROM Vehiculos v
                    INNER JOIN Categorias c ON v.CategoriaId = c.Id
                    INNER JOIN Sucursales s ON v.SucursalId = s.Id
                    WHERE v.Estado = @EstadoDisponible AND v.SucursalId = @SucursalId
                    ORDER BY c.Nombre, v.Marca, v.Modelo"
                : @"SELECT v.Id, v.Marca, v.Modelo, v.Anio, v.Patente, v.Color,
                           v.Kilometraje, v.Estado, v.CategoriaId, v.SucursalId,
                           c.Nombre AS CategoriaNombre, c.PrecioDiario AS CategoriaPrecioDiario,
                           s.Nombre AS SucursalNombre, s.Ciudad AS SucursalCiudad
                    FROM Vehiculos v
                    INNER JOIN Categorias c ON v.CategoriaId = c.Id
                    INNER JOIN Sucursales s ON v.SucursalId = s.Id
                    WHERE v.Estado = @EstadoDisponible
                    ORDER BY c.Nombre, v.Marca, v.Modelo";

            var parameters = sucursalId.HasValue
                ? new SqlParameter[]
                {
                    new SqlParameter("@EstadoDisponible", Vehiculo.ESTADO_DISPONIBLE),
                    new SqlParameter("@SucursalId", sucursalId.Value)
                }
                : new SqlParameter[]
                {
                    new SqlParameter("@EstadoDisponible", Vehiculo.ESTADO_DISPONIBLE)
                };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return VehiculoMapper.MapList(dataTable);
        }

        public Vehiculo? GetById(int id)
        {
            string query = @"
                SELECT v.Id, v.Marca, v.Modelo, v.Anio, v.Patente, v.Color,
                       v.Kilometraje, v.Estado, v.CategoriaId, v.SucursalId,
                       c.Nombre AS CategoriaNombre, c.PrecioDiario AS CategoriaPrecioDiario,
                       s.Nombre AS SucursalNombre, s.Ciudad AS SucursalCiudad
                FROM Vehiculos v
                INNER JOIN Categorias c ON v.CategoriaId = c.Id
                INNER JOIN Sucursales s ON v.SucursalId = s.Id
                WHERE v.Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return VehiculoMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public Vehiculo? GetByPatente(string patente)
        {
            string query = @"
                SELECT v.Id, v.Marca, v.Modelo, v.Anio, v.Patente, v.Color,
                       v.Kilometraje, v.Estado, v.CategoriaId, v.SucursalId,
                       c.Nombre AS CategoriaNombre, c.PrecioDiario AS CategoriaPrecioDiario,
                       s.Nombre AS SucursalNombre, s.Ciudad AS SucursalCiudad
                FROM Vehiculos v
                INNER JOIN Categorias c ON v.CategoriaId = c.Id
                INNER JOIN Sucursales s ON v.SucursalId = s.Id
                WHERE v.Patente = @Patente";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Patente", patente)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return VehiculoMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public bool ExistsPatente(string patente, int? excludeId = null)
        {
            string query = excludeId.HasValue
                ? "SELECT COUNT(*) FROM Vehiculos WHERE Patente = @Patente AND Id != @ExcludeId"
                : "SELECT COUNT(*) FROM Vehiculos WHERE Patente = @Patente";

            var parameters = excludeId.HasValue
                ? new SqlParameter[]
                {
                    new SqlParameter("@Patente", patente),
                    new SqlParameter("@ExcludeId", excludeId.Value)
                }
                : new SqlParameter[]
                {
                    new SqlParameter("@Patente", patente)
                };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }

        public int Insert(Vehiculo vehiculo)
        {
            string query = @"
                INSERT INTO Vehiculos (Marca, Modelo, Anio, Patente, Color,
                                      Kilometraje, Estado, CategoriaId, SucursalId)
                VALUES (@Marca, @Modelo, @Anio, @Patente, @Color,
                        @Kilometraje, @Estado, @CategoriaId, @SucursalId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Marca", vehiculo.Marca),
                new SqlParameter("@Modelo", vehiculo.Modelo),
                new SqlParameter("@Anio", vehiculo.Anio),
                new SqlParameter("@Patente", vehiculo.Patente),
                new SqlParameter("@Color", vehiculo.Color),
                new SqlParameter("@Kilometraje", vehiculo.Kilometraje),
                new SqlParameter("@Estado", (int)vehiculo.Estado),
                new SqlParameter("@CategoriaId", vehiculo.CategoriaId),
                new SqlParameter("@SucursalId", vehiculo.SucursalId)
            };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result);
        }

        public bool Update(Vehiculo vehiculo)
        {
            string query = @"
                UPDATE Vehiculos
                SET Marca = @Marca,
                    Modelo = @Modelo,
                    Anio = @Anio,
                    Patente = @Patente,
                    Color = @Color,
                    Kilometraje = @Kilometraje,
                    Estado = @Estado,
                    CategoriaId = @CategoriaId,
                    SucursalId = @SucursalId
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", vehiculo.Id),
                new SqlParameter("@Marca", vehiculo.Marca),
                new SqlParameter("@Modelo", vehiculo.Modelo),
                new SqlParameter("@Anio", vehiculo.Anio),
                new SqlParameter("@Patente", vehiculo.Patente),
                new SqlParameter("@Color", vehiculo.Color),
                new SqlParameter("@Kilometraje", vehiculo.Kilometraje),
                new SqlParameter("@Estado", (int)vehiculo.Estado),
                new SqlParameter("@CategoriaId", vehiculo.CategoriaId),
                new SqlParameter("@SucursalId", vehiculo.SucursalId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool UpdateEstado(int id, int estado, TransactionManager? transaction = null)
        {
            string query = "UPDATE Vehiculos SET Estado = @Estado WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Estado", estado)
            };

            if (transaction != null)
            {
                using (var command = new SqlCommand(query, transaction.Connection, transaction.Transaction))
                {
                    command.Parameters.AddRange(parameters);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            else
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            // Cambiar estado a Inactivo
            string query = "UPDATE Vehiculos SET Estado = @EstadoInactivo WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@EstadoInactivo", Vehiculo.ESTADO_INACTIVO)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}
