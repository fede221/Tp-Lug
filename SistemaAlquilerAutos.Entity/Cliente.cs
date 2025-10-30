namespace SistemaAlquilerAutos.Entity
{
    public class Cliente
    {
        public int Id { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public bool Activo { get; set; }

        // Propiedad calculada
        public string NombreCompleto => $"{Apellido}, {Nombre}";

        public int Edad
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        // Constructor
        public Cliente()
        {
            Activo = true;
        }

        public override string ToString()
        {
            return $"{NombreCompleto} (DNI: {DNI})";
        }
    }
}
