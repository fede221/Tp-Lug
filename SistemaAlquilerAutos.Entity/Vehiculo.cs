namespace SistemaAlquilerAutos.Entity
{
    public enum EstadoVehiculo
    {
        Disponible = 1,
        Alquilado = 2,
        Mantenimiento = 3,
        Inactivo = 4
    }

    public class Vehiculo
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Patente { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Kilometraje { get; set; }
        public EstadoVehiculo Estado { get; set; }

        // Foreign Keys
        public int CategoriaId { get; set; }
        public int SucursalId { get; set; }

        // Navigation Properties (opcional para mostrar información completa)
        public Categoria? Categoria { get; set; }
        public Sucursal? Sucursal { get; set; }

        // Constructor
        public Vehiculo()
        {
            Estado = EstadoVehiculo.Disponible;
        }

        public override string ToString()
        {
            return $"{Marca} {Modelo} ({Patente})";
        }
    }
}
