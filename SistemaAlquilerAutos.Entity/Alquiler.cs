namespace SistemaAlquilerAutos.Entity
{
    public enum EstadoAlquiler
    {
        Activo = 1,
        Completado = 2,
        Cancelado = 3
    }

    public class Alquiler
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime FechaDevolucionPrevista { get; set; }
        public int KilometrajeInicio { get; set; }
        public int? KilometrajeFin { get; set; }
        public decimal PrecioTotal { get; set; }
        public EstadoAlquiler Estado { get; set; }
        public string? Observaciones { get; set; }

        // Foreign Keys
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int SucursalRetiroId { get; set; }
        public int? SucursalDevolucionId { get; set; }

        // Navigation Properties
        public Cliente? Cliente { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public Sucursal? SucursalRetiro { get; set; }
        public Sucursal? SucursalDevolucion { get; set; }

        // Propiedades calculadas
        public int DiasAlquiler
        {
            get
            {
                if (FechaFin.HasValue)
                {
                    return (FechaFin.Value - FechaInicio).Days;
                }
                return (FechaDevolucionPrevista - FechaInicio).Days;
            }
        }

        public bool TieneRetraso
        {
            get
            {
                if (!FechaFin.HasValue && DateTime.Now > FechaDevolucionPrevista)
                {
                    return true;
                }
                return false;
            }
        }

        // Constructor
        public Alquiler()
        {
            Estado = EstadoAlquiler.Activo;
            FechaInicio = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Alquiler #{Id} - Cliente: {Cliente?.NombreCompleto ?? "N/A"}";
        }
    }
}
