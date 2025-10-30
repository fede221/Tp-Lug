namespace SistemaAlquilerAutos.Entity
{
    /// <summary>
    /// Estados posibles de un vehículo
    /// 1 = Disponible, 2 = Alquilado, 3 = Mantenimiento, 4 = Inactivo
    /// </summary>
    public class Vehiculo
    {
        // Constantes para estados
        public const int ESTADO_DISPONIBLE = 1;
        public const int ESTADO_ALQUILADO = 2;
        public const int ESTADO_MANTENIMIENTO = 3;
        public const int ESTADO_INACTIVO = 4;

        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Patente { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Kilometraje { get; set; }
        public int Estado { get; set; }

        // Foreign Keys
        public int CategoriaId { get; set; }
        public int SucursalId { get; set; }

        // Navigation Properties (opcional para mostrar información completa)
        public Categoria? Categoria { get; set; }
        public Sucursal? Sucursal { get; set; }

        // Propiedad auxiliar para obtener el nombre del estado
        public string EstadoNombre
        {
            get
            {
                return Estado switch
                {
                    ESTADO_DISPONIBLE => "Disponible",
                    ESTADO_ALQUILADO => "Alquilado",
                    ESTADO_MANTENIMIENTO => "Mantenimiento",
                    ESTADO_INACTIVO => "Inactivo",
                    _ => "Desconocido"
                };
            }
        }

        // Constructor
        public Vehiculo()
        {
            Estado = ESTADO_DISPONIBLE;
        }

        public override string ToString()
        {
            return $"{Marca} {Modelo} ({Patente})";
        }
    }
}
