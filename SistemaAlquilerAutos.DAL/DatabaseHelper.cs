using System.Data;
using System.Data.SqlClient;

namespace SistemaAlquilerAutos.DAL
{
    public static class DatabaseHelper
    {
        private static string? _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión no ha sido configurada. Use SetConnectionString() primero.");
            }
            return _connectionString;
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        connection.Open();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }
    }
}
