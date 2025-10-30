namespace SistemaAlquilerAutos.Entity
{
    /// <summary>
    /// Estados posibles de un alquiler
    /// 1 = Activo, 2 = Completado, 3 = Cancelado
    /// </summary>
    public class Alquiler
    {
        // Constantes para estados
        public const int ESTADO_ACTIVO = 1;
        public const int ESTADO_COMPLETADO = 2;
        public const int ESTADO_CANCELADO = 3;

        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime FechaDevolucionPrevista { get; set; }
        public int KilometrajeInicio { get; set; }
        public int? KilometrajeFin { get; set; }
        public decimal PrecioTotal { get; set; }
        public int Estado { get; set; }
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

        // Propiedad auxiliar para obtener el nombre del estado
        public string EstadoNombre
        {
            get
            {
                return Estado switch
                {
                    ESTADO_ACTIVO => "Activo",
                    ESTADO_COMPLETADO => "Completado",
                    ESTADO_CANCELADO => "Cancelado",
                    _ => "Desconocido"
                };
            }
        }

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
            Estado = ESTADO_ACTIVO;
            FechaInicio = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Alquiler #{Id} - Cliente: {Cliente?.NombreCompleto ?? "N/A"}";
        }
    }
}
