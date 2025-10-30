using System.Data;
using System.Data.SqlClient;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.Mapper;

namespace SistemaAlquilerAutos.DAL
{
    public class ClienteDAL
    {
        public List<Cliente> GetAll()
        {
            string query = @"
                SELECT Id, DNI, Nombre, Apellido, Email, Telefono,
                       FechaNacimiento, Direccion, Ciudad, Activo
                FROM Clientes
                WHERE Activo = 1
                ORDER BY Apellido, Nombre";

            var dataTable = DatabaseHelper.ExecuteQuery(query);
            return ClienteMapper.MapList(dataTable);
        }

        public Cliente? GetById(int id)
        {
            string query = @"
                SELECT Id, DNI, Nombre, Apellido, Email, Telefono,
                       FechaNacimiento, Direccion, Ciudad, Activo
                FROM Clientes
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return ClienteMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public Cliente? GetByDNI(string dni)
        {
            string query = @"
                SELECT Id, DNI, Nombre, Apellido, Email, Telefono,
                       FechaNacimiento, Direccion, Ciudad, Activo
                FROM Clientes
                WHERE DNI = @DNI";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DNI", dni)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return ClienteMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public bool ExistsDNI(string dni, int? excludeId = null)
        {
            string query = excludeId.HasValue
                ? "SELECT COUNT(*) FROM Clientes WHERE DNI = @DNI AND Id != @ExcludeId"
                : "SELECT COUNT(*) FROM Clientes WHERE DNI = @DNI";

            var parameters = excludeId.HasValue
                ? new SqlParameter[]
                {
                    new SqlParameter("@DNI", dni),
                    new SqlParameter("@ExcludeId", excludeId.Value)
                }
                : new SqlParameter[]
                {
                    new SqlParameter("@DNI", dni)
                };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }

        public int Insert(Cliente cliente)
        {
            string query = @"
                INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono,
                                     FechaNacimiento, Direccion, Ciudad, Activo)
                VALUES (@DNI, @Nombre, @Apellido, @Email, @Telefono,
                        @FechaNacimiento, @Direccion, @Ciudad, @Activo);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DNI", cliente.DNI),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@Email", cliente.Email),
                new SqlParameter("@Telefono", cliente.Telefono),
                new SqlParameter("@FechaNacimiento", cliente.FechaNacimiento),
                new SqlParameter("@Direccion", cliente.Direccion),
                new SqlParameter("@Ciudad", cliente.Ciudad),
                new SqlParameter("@Activo", cliente.Activo)
            };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result);
        }

        public bool Update(Cliente cliente)
        {
            string query = @"
                UPDATE Clientes
                SET DNI = @DNI,
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Email = @Email,
                    Telefono = @Telefono,
                    FechaNacimiento = @FechaNacimiento,
                    Direccion = @Direccion,
                    Ciudad = @Ciudad,
                    Activo = @Activo
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", cliente.Id),
                new SqlParameter("@DNI", cliente.DNI),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@Email", cliente.Email),
                new SqlParameter("@Telefono", cliente.Telefono),
                new SqlParameter("@FechaNacimiento", cliente.FechaNacimiento),
                new SqlParameter("@Direccion", cliente.Direccion),
                new SqlParameter("@Ciudad", cliente.Ciudad),
                new SqlParameter("@Activo", cliente.Activo)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            // Soft delete
            string query = "UPDATE Clientes SET Activo = 0 WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public List<Cliente> Search(string searchText)
        {
            string query = @"
                SELECT Id, DNI, Nombre, Apellido, Email, Telefono,
                       FechaNacimiento, Direccion, Ciudad, Activo
                FROM Clientes
                WHERE (Nombre LIKE @Search OR Apellido LIKE @Search OR DNI LIKE @Search)
                      AND Activo = 1
                ORDER BY Apellido, Nombre";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Search", "%" + searchText + "%")
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
            return ClienteMapper.MapList(dataTable);
        }
    }
}
