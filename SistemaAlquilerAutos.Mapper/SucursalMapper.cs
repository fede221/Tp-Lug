using System.Data;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.Mapper
{
    public static class SucursalMapper
    {
        public static Sucursal Map(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new Sucursal
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString() ?? string.Empty,
                Direccion = row["Direccion"].ToString() ?? string.Empty,
                Ciudad = row["Ciudad"].ToString() ?? string.Empty,
                Telefono = row["Telefono"].ToString() ?? string.Empty,
                Email = row["Email"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public static List<Sucursal> MapList(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var list = new List<Sucursal>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(Map(row));
            }
            return list;
        }
    }
}
