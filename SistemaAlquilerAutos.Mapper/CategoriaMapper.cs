using System.Data;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.Mapper
{
    public static class CategoriaMapper
    {
        public static Categoria Map(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new Categoria
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString() ?? string.Empty,
                Descripcion = row["Descripcion"].ToString() ?? string.Empty,
                PrecioDiario = Convert.ToDecimal(row["PrecioDiario"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public static List<Categoria> MapList(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var list = new List<Categoria>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(Map(row));
            }
            return list;
        }
    }
}
