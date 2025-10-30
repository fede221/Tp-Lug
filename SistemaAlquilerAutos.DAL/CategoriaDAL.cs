using System.Data;
using System.Data.SqlClient;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.Mapper;

namespace SistemaAlquilerAutos.DAL
{
    public class CategoriaDAL
    {
        public List<Categoria> GetAll()
        {
            string query = @"
                SELECT Id, Nombre, Descripcion, PrecioDiario, Activo
                FROM Categorias
                WHERE Activo = 1
                ORDER BY Nombre";

            var dataTable = DatabaseHelper.ExecuteQuery(query);
            return CategoriaMapper.MapList(dataTable);
        }

        public Categoria? GetById(int id)
        {
            string query = @"
                SELECT Id, Nombre, Descripcion, PrecioDiario, Activo
                FROM Categorias
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return CategoriaMapper.Map(dataTable.Rows[0]);
            }

            return null;
        }

        public int Insert(Categoria categoria)
        {
            string query = @"
                INSERT INTO Categorias (Nombre, Descripcion, PrecioDiario, Activo)
                VALUES (@Nombre, @Descripcion, @PrecioDiario, @Activo);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", categoria.Nombre),
                new SqlParameter("@Descripcion", categoria.Descripcion),
                new SqlParameter("@PrecioDiario", categoria.PrecioDiario),
                new SqlParameter("@Activo", categoria.Activo)
            };

            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result);
        }

        public int Insert(Categoria categoria, TransactionManager transaction)
        {
            string query = @"
                INSERT INTO Categorias (Nombre, Descripcion, PrecioDiario, Activo)
                VALUES (@Nombre, @Descripcion, @PrecioDiario, @Activo);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var command = new SqlCommand(query, transaction.Connection, transaction.Transaction))
            {
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                command.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                command.Parameters.AddWithValue("@PrecioDiario", categoria.PrecioDiario);
                command.Parameters.AddWithValue("@Activo", categoria.Activo);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Categoria categoria)
        {
            string query = @"
                UPDATE Categorias
                SET Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    PrecioDiario = @PrecioDiario,
                    Activo = @Activo
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", categoria.Id),
                new SqlParameter("@Nombre", categoria.Nombre),
                new SqlParameter("@Descripcion", categoria.Descripcion),
                new SqlParameter("@PrecioDiario", categoria.PrecioDiario),
                new SqlParameter("@Activo", categoria.Activo)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            // Soft delete
            string query = "UPDATE Categorias SET Activo = 0 WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}
