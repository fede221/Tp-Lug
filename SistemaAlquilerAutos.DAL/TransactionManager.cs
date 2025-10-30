using System.Data.SqlClient;

namespace SistemaAlquilerAutos.DAL
{
    /// <summary>
    /// Administrador de transacciones para operaciones que requieren atomicidad
    /// </summary>
    public class TransactionManager : IDisposable
    {
        private SqlConnection? _connection;
        private SqlTransaction? _transaction;
        private bool _disposed = false;

        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = DatabaseHelper.CreateConnection();
                    _connection.Open();
                }
                return _connection;
            }
        }

        public SqlTransaction Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = Connection.BeginTransaction();
                }
                return _transaction;
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Si hay una transacción activa, hacer rollback
                    if (_transaction != null)
                    {
                        _transaction.Rollback();
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    // Cerrar la conexión
                    if (_connection != null)
                    {
                        if (_connection.State == System.Data.ConnectionState.Open)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}
