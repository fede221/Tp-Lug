using System.Data;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.Mapper
{
    public static class ClienteMapper
    {
        public static Cliente Map(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new Cliente
            {
                Id = Convert.ToInt32(row["Id"]),
                DNI = row["DNI"].ToString() ?? string.Empty,
                Nombre = row["Nombre"].ToString() ?? string.Empty,
                Apellido = row["Apellido"].ToString() ?? string.Empty,
                Email = row["Email"].ToString() ?? string.Empty,
                Telefono = row["Telefono"].ToString() ?? string.Empty,
                FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                Direccion = row["Direccion"].ToString() ?? string.Empty,
                Ciudad = row["Ciudad"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public static List<Cliente> MapList(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var list = new List<Cliente>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(Map(row));
            }
            return list;
        }
    }
}
