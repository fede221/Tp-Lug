namespace SistemaAlquilerAutos.Entity
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }

        // Constructor
        public Sucursal()
        {
            Activo = true;
        }

        public override string ToString()
        {
            return $"{Nombre} - {Ciudad}";
        }
    }
}
