using System.Data;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.Mapper
{
    public static class AlquilerMapper
    {
        public static Alquiler Map(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var alquiler = new Alquiler
            {
                Id = Convert.ToInt32(row["Id"]),
                FechaInicio = Convert.ToDateTime(row["FechaInicio"]),
                FechaFin = row["FechaFin"] != DBNull.Value ? Convert.ToDateTime(row["FechaFin"]) : null,
                FechaDevolucionPrevista = Convert.ToDateTime(row["FechaDevolucionPrevista"]),
                KilometrajeInicio = Convert.ToInt32(row["KilometrajeInicio"]),
                KilometrajeFin = row["KilometrajeFin"] != DBNull.Value ? Convert.ToInt32(row["KilometrajeFin"]) : null,
                PrecioTotal = Convert.ToDecimal(row["PrecioTotal"]),
                Estado = Convert.ToInt32(row["Estado"]),
                Observaciones = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : null,
                ClienteId = Convert.ToInt32(row["ClienteId"]),
                VehiculoId = Convert.ToInt32(row["VehiculoId"]),
                SucursalRetiroId = Convert.ToInt32(row["SucursalRetiroId"]),
                SucursalDevolucionId = row["SucursalDevolucionId"] != DBNull.Value ? Convert.ToInt32(row["SucursalDevolucionId"]) : null
            };

            // Si la consulta incluye información del Cliente
            if (row.Table.Columns.Contains("ClienteNombre"))
            {
                alquiler.Cliente = new Cliente
                {
                    Id = alquiler.ClienteId,
                    Nombre = row["ClienteNombre"].ToString() ?? string.Empty,
                    Apellido = row["ClienteApellido"].ToString() ?? string.Empty,
                    DNI = row.Table.Columns.Contains("ClienteDNI") ? row["ClienteDNI"].ToString() ?? string.Empty : string.Empty
                };
            }

            // Si la consulta incluye información del Vehículo
            if (row.Table.Columns.Contains("VehiculoMarca"))
            {
                alquiler.Vehiculo = new Vehiculo
                {
                    Id = alquiler.VehiculoId,
                    Marca = row["VehiculoMarca"].ToString() ?? string.Empty,
                    Modelo = row["VehiculoModelo"].ToString() ?? string.Empty,
                    Patente = row.Table.Columns.Contains("VehiculoPatente") ? row["VehiculoPatente"].ToString() ?? string.Empty : string.Empty
                };
            }

            return alquiler;
        }

        public static List<Alquiler> MapList(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var list = new List<Alquiler>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(Map(row));
            }
            return list;
        }
    }
}
