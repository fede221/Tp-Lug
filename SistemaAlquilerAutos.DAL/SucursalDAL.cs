using System.Data;
using System.Data.SqlClient;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.Mapper;

namespace SistemaAlquilerAutos.DAL
{
    public class SucursalDAL
    {
        public List<Sucursal> GetAll()
        {
            string query = @"
                SELECT Id, Nombre, Direccion, Ciudad, Telefono, Email, Activo
                FROM Sucursales
                WHERE Activo = 1
                ORDER BY Ciudad, Nombre";

            var dataTable = DatabaseHelper.ExecuteQuery(query);
            return SucursalMapper.MapList(dataTable);
        }

        public Sucursal? GetById(int id)
        {
            string query = @"
                SELECT Id, Nombre, Direccion, Ciudad, Telefono, Email, Activo
                FROM Sucursales
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return SucursalMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public List<Sucursal> GetByCity(string ciudad)
        {
            string query = @"
                SELECT Id, Nombre, Direccion, Ciudad, Telefono, Email, Activo
                FROM Sucursales
                WHERE Ciudad = @Ciudad AND Activo = 1
                ORDER BY Nombre";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Ciudad", ciudad)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return SucursalMapper.MapList(dataTable);
        }

        public int Insert(Sucursal sucursal)
        {
            string query = @"
                INSERT INTO Sucursales (Nombre, Direccion, Ciudad, Telefono, Email, Activo)
                VALUES (@Nombre, @Direccion, @Ciudad, @Telefono, @Email, @Activo);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", sucursal.Nombre),
                new SqlParameter("@Direccion", sucursal.Direccion),
                new SqlParameter("@Ciudad", sucursal.Ciudad),
                new SqlParameter("@Telefono", sucursal.Telefono),
                new SqlParameter("@Email", sucursal.Email),
                new SqlParameter("@Activo", sucursal.Activo)
            };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result);
        }

        public bool Update(Sucursal sucursal)
        {
            string query = @"
                UPDATE Sucursales
                SET Nombre = @Nombre,
                    Direccion = @Direccion,
                    Ciudad = @Ciudad,
                    Telefono = @Telefono,
                    Email = @Email,
                    Activo = @Activo
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", sucursal.Id),
                new SqlParameter("@Nombre", sucursal.Nombre),
                new SqlParameter("@Direccion", sucursal.Direccion),
                new SqlParameter("@Ciudad", sucursal.Ciudad),
                new SqlParameter("@Telefono", sucursal.Telefono),
                new SqlParameter("@Email", sucursal.Email),
                new SqlParameter("@Activo", sucursal.Activo)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            // Soft delete
            string query = "UPDATE Sucursales SET Activo = 0 WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}
