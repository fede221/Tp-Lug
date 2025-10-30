using System.Data;
using System.Data.SqlClient;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.Mapper;

namespace SistemaAlquilerAutos.DAL
{
    public class AlquilerDAL
    {
        public List<Alquiler> GetAll()
        {
            string query = @"
                SELECT a.Id, a.FechaInicio, a.FechaFin, a.FechaDevolucionPrevista,
                       a.KilometrajeInicio, a.KilometrajeFin, a.PrecioTotal, a.Estado,
                       a.Observaciones, a.ClienteId, a.VehiculoId, a.SucursalRetiroId,
                       a.SucursalDevolucionId,
                       c.Nombre AS ClienteNombre, c.Apellido AS ClienteApellido, c.DNI AS ClienteDNI,
                       v.Marca AS VehiculoMarca, v.Modelo AS VehiculoModelo, v.Patente AS VehiculoPatente
                FROM Alquileres a
                INNER JOIN Clientes c ON a.ClienteId = c.Id
                INNER JOIN Vehiculos v ON a.VehiculoId = v.Id
                ORDER BY a.FechaInicio DESC";

            var dataTable = DatabaseHelper.ExecuteQuery(query);
            return AlquilerMapper.MapList(dataTable);
        }

        public List<Alquiler> GetActivos()
        {
            string query = @"
                SELECT a.Id, a.FechaInicio, a.FechaFin, a.FechaDevolucionPrevista,
                       a.KilometrajeInicio, a.KilometrajeFin, a.PrecioTotal, a.Estado,
                       a.Observaciones, a.ClienteId, a.VehiculoId, a.SucursalRetiroId,
                       a.SucursalDevolucionId,
                       c.Nombre AS ClienteNombre, c.Apellido AS ClienteApellido, c.DNI AS ClienteDNI,
                       v.Marca AS VehiculoMarca, v.Modelo AS VehiculoModelo, v.Patente AS VehiculoPatente
                FROM Alquileres a
                INNER JOIN Clientes c ON a.ClienteId = c.Id
                INNER JOIN Vehiculos v ON a.VehiculoId = v.Id
                WHERE a.Estado = @EstadoActivo
                ORDER BY a.FechaDevolucionPrevista";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@EstadoActivo", (int)EstadoAlquiler.Activo)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return AlquilerMapper.MapList(dataTable);
        }

        public List<Alquiler> GetByCliente(int clienteId)
        {
            string query = @"
                SELECT a.Id, a.FechaInicio, a.FechaFin, a.FechaDevolucionPrevista,
                       a.KilometrajeInicio, a.KilometrajeFin, a.PrecioTotal, a.Estado,
                       a.Observaciones, a.ClienteId, a.VehiculoId, a.SucursalRetiroId,
                       a.SucursalDevolucionId,
                       c.Nombre AS ClienteNombre, c.Apellido AS ClienteApellido, c.DNI AS ClienteDNI,
                       v.Marca AS VehiculoMarca, v.Modelo AS VehiculoModelo, v.Patente AS VehiculoPatente
                FROM Alquileres a
                INNER JOIN Clientes c ON a.ClienteId = c.Id
                INNER JOIN Vehiculos v ON a.VehiculoId = v.Id
                WHERE a.ClienteId = @ClienteId
                ORDER BY a.FechaInicio DESC";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ClienteId", clienteId)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return AlquilerMapper.MapList(dataTable);
        }

        public Alquiler? GetById(int id)
        {
            string query = @"
                SELECT a.Id, a.FechaInicio, a.FechaFin, a.FechaDevolucionPrevista,
                       a.KilometrajeInicio, a.KilometrajeFin, a.PrecioTotal, a.Estado,
                       a.Observaciones, a.ClienteId, a.VehiculoId, a.SucursalRetiroId,
                       a.SucursalDevolucionId,
                       c.Nombre AS ClienteNombre, c.Apellido AS ClienteApellido, c.DNI AS ClienteDNI,
                       v.Marca AS VehiculoMarca, v.Modelo AS VehiculoModelo, v.Patente AS VehiculoPatente
                FROM Alquileres a
                INNER JOIN Clientes c ON a.ClienteId = c.Id
                INNER JOIN Vehiculos v ON a.VehiculoId = v.Id
                WHERE a.Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return AlquilerMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public Alquiler? GetAlquilerActivoByVehiculo(int vehiculoId)
        {
            string query = @"
                SELECT a.Id, a.FechaInicio, a.FechaFin, a.FechaDevolucionPrevista,
                       a.KilometrajeInicio, a.KilometrajeFin, a.PrecioTotal, a.Estado,
                       a.Observaciones, a.ClienteId, a.VehiculoId, a.SucursalRetiroId,
                       a.SucursalDevolucionId,
                       c.Nombre AS ClienteNombre, c.Apellido AS ClienteApellido, c.DNI AS ClienteDNI,
                       v.Marca AS VehiculoMarca, v.Modelo AS VehiculoModelo, v.Patente AS VehiculoPatente
                FROM Alquileres a
                INNER JOIN Clientes c ON a.ClienteId = c.Id
                INNER JOIN Vehiculos v ON a.VehiculoId = v.Id
                WHERE a.VehiculoId = @VehiculoId AND a.Estado = @EstadoActivo";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@VehiculoId", vehiculoId),
                new SqlParameter("@EstadoActivo", (int)EstadoAlquiler.Activo)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return AlquilerMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public int Insert(Alquiler alquiler, TransactionManager transaction)
        {
            string query = @"
                INSERT INTO Alquileres (ClienteId, VehiculoId, FechaInicio, FechaDevolucionPrevista,
                                       SucursalRetiroId, KilometrajeInicio, PrecioTotal, Estado, Observaciones)
                VALUES (@ClienteId, @VehiculoId, @FechaInicio, @FechaDevolucionPrevista,
                        @SucursalRetiroId, @KilometrajeInicio, @PrecioTotal, @Estado, @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var command = new SqlCommand(query, transaction.Connection, transaction.Transaction))
            {
                command.Parameters.AddWithValue("@ClienteId", alquiler.ClienteId);
                command.Parameters.AddWithValue("@VehiculoId", alquiler.VehiculoId);
                command.Parameters.AddWithValue("@FechaInicio", alquiler.FechaInicio);
                command.Parameters.AddWithValue("@FechaDevolucionPrevista", alquiler.FechaDevolucionPrevista);
                command.Parameters.AddWithValue("@SucursalRetiroId", alquiler.SucursalRetiroId);
                command.Parameters.AddWithValue("@KilometrajeInicio", alquiler.KilometrajeInicio);
                command.Parameters.AddWithValue("@PrecioTotal", alquiler.PrecioTotal);
                command.Parameters.AddWithValue("@Estado", (int)alquiler.Estado);
                command.Parameters.AddWithValue("@Observaciones", (object?)alquiler.Observaciones ?? DBNull.Value);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool FinalizarAlquiler(int id, DateTime fechaFin, int kilometrajeFin,
                                     int? sucursalDevolucionId, string? observaciones,
                                     TransactionManager transaction)
        {
            string query = @"
                UPDATE Alquileres
                SET FechaFin = @FechaFin,
                    KilometrajeFin = @KilometrajeFin,
                    SucursalDevolucionId = @SucursalDevolucionId,
                    Observaciones = @Observaciones,
                    Estado = @EstadoCompletado
                WHERE Id = @Id";

            using (var command = new SqlCommand(query, transaction.Connection, transaction.Transaction))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);
                command.Parameters.AddWithValue("@KilometrajeFin", kilometrajeFin);
                command.Parameters.AddWithValue("@SucursalDevolucionId", (object?)sucursalDevolucionId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Observaciones", (object?)observaciones ?? DBNull.Value);
                command.Parameters.AddWithValue("@EstadoCompletado", (int)EstadoAlquiler.Completado);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool CancelarAlquiler(int id, string? motivoCancelacion, TransactionManager transaction)
        {
            string query = @"
                UPDATE Alquileres
                SET Estado = @EstadoCancelado,
                    Observaciones = @Observaciones
                WHERE Id = @Id";

            using (var command = new SqlCommand(query, transaction.Connection, transaction.Transaction))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@EstadoCancelado", (int)EstadoAlquiler.Cancelado);
                command.Parameters.AddWithValue("@Observaciones", (object?)motivoCancelacion ?? DBNull.Value);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
