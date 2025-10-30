using System.Configuration;
using SistemaAlquilerAutos.DAL;

namespace SistemaAlquilerAutos.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configurar la aplicación
            ApplicationConfiguration.Initialize();

            try
            {
                // Configurar la cadena de conexión desde App.config
                string? connectionString = ConfigurationManager.ConnectionStrings["SistemaAlquilerAutos"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show(
                        "No se encontró la cadena de conexión en App.config. " +
                        "Por favor, configure la conexión a la base de datos.",
                        "Error de Configuración",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                DatabaseHelper.SetConnectionString(connectionString);

                // Iniciar la aplicación con el formulario principal
                Application.Run(new FormPrincipal());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicación:\n{ex.Message}",
                    "Error Fatal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
