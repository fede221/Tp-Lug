namespace SistemaAlquilerAutos.Entity
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioDiario { get; set; }
        public bool Activo { get; set; }

        // Constructor
        public Categoria()
        {
            Activo = true;
        }

        public override string ToString()
        {
            return $"{Nombre} - ${PrecioDiario}/día";
        }
    }
}
