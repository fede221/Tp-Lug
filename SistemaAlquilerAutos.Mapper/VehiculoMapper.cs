using System.Data;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.Mapper
{
    public static class VehiculoMapper
    {
        public static Vehiculo Map(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var vehiculo = new Vehiculo
            {
                Id = Convert.ToInt32(row["Id"]),
                Marca = row["Marca"].ToString() ?? string.Empty,
                Modelo = row["Modelo"].ToString() ?? string.Empty,
                Anio = Convert.ToInt32(row["Anio"]),
                Patente = row["Patente"].ToString() ?? string.Empty,
                Color = row["Color"].ToString() ?? string.Empty,
                Kilometraje = Convert.ToInt32(row["Kilometraje"]),
                Estado = (EstadoVehiculo)Convert.ToInt32(row["Estado"]),
                CategoriaId = Convert.ToInt32(row["CategoriaId"]),
                SucursalId = Convert.ToInt32(row["SucursalId"])
            };

            // Si la consulta incluye información de Categoria
            if (row.Table.Columns.Contains("CategoriaNombre"))
            {
                vehiculo.Categoria = new Categoria
                {
                    Id = vehiculo.CategoriaId,
                    Nombre = row["CategoriaNombre"].ToString() ?? string.Empty,
                    PrecioDiario = row.Table.Columns.Contains("CategoriaPrecioDiario")
                        ? Convert.ToDecimal(row["CategoriaPrecioDiario"])
                        : 0
                };
            }

            // Si la consulta incluye información de Sucursal
            if (row.Table.Columns.Contains("SucursalNombre"))
            {
                vehiculo.Sucursal = new Sucursal
                {
                    Id = vehiculo.SucursalId,
                    Nombre = row["SucursalNombre"].ToString() ?? string.Empty,
                    Ciudad = row.Table.Columns.Contains("SucursalCiudad")
                        ? row["SucursalCiudad"].ToString() ?? string.Empty
                        : string.Empty
                };
            }

            return vehiculo;
        }

        public static List<Vehiculo> MapList(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var list = new List<Vehiculo>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(Map(row));
            }
            return list;
        }
    }
}
