using System.Text.RegularExpressions;
using SistemaAlquilerAutos.DAL;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.BLL
{
    public class VehiculoBLL
    {
        private readonly VehiculoDAL _vehiculoDAL;
        private readonly CategoriaDAL _categoriaDAL;
        private readonly SucursalDAL _sucursalDAL;

        public VehiculoBLL()
        {
            _vehiculoDAL = new VehiculoDAL();
            _categoriaDAL = new CategoriaDAL();
            _sucursalDAL = new SucursalDAL();
        }

        public List<Vehiculo> GetAll()
        {
            try
            {
                return _vehiculoDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los vehículos.", ex);
            }
        }

        public List<Vehiculo> GetDisponibles(int? sucursalId = null)
        {
            try
            {
                return _vehiculoDAL.GetDisponibles(sucursalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los vehículos disponibles.", ex);
            }
        }

        public Vehiculo GetById(int id)
        {
            try
            {
                var vehiculo = _vehiculoDAL.GetById(id);
                if (vehiculo == null)
                {
                    throw new EntityNotFoundException("Vehiculo", id);
                }
                return vehiculo;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el vehículo con ID {id}.", ex);
            }
        }

        public Vehiculo? GetByPatente(string patente)
        {
            try
            {
                return _vehiculoDAL.GetByPatente(patente);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al buscar vehículo por patente {patente}.", ex);
            }
        }

        public int Insert(Vehiculo vehiculo)
        {
            try
            {
                // Validaciones de negocio
                ValidarVehiculo(vehiculo);

                // Verificar que existe la categoría
                var categoria = _categoriaDAL.GetById(vehiculo.CategoriaId);
                if (categoria == null)
                {
                    throw new BusinessRuleException($"La categoría con ID {vehiculo.CategoriaId} no existe.");
                }

                // Verificar que existe la sucursal
                var sucursal = _sucursalDAL.GetById(vehiculo.SucursalId);
                if (sucursal == null)
                {
                    throw new BusinessRuleException($"La sucursal con ID {vehiculo.SucursalId} no existe.");
                }

                // Verificar que no exista la patente
                if (_vehiculoDAL.ExistsPatente(vehiculo.Patente))
                {
                    throw new DuplicateEntityException($"Ya existe un vehículo con patente {vehiculo.Patente}.");
                }

                return _vehiculoDAL.Insert(vehiculo);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al insertar el vehículo.", ex);
            }
        }

        public void Update(Vehiculo vehiculo)
        {
            try
            {
                // Validaciones de negocio
                ValidarVehiculo(vehiculo);

                // Verificar que existe
                var existente = _vehiculoDAL.GetById(vehiculo.Id);
                if (existente == null)
                {
                    throw new EntityNotFoundException("Vehiculo", vehiculo.Id);
                }

                // Verificar que existe la categoría
                var categoria = _categoriaDAL.GetById(vehiculo.CategoriaId);
                if (categoria == null)
                {
                    throw new BusinessRuleException($"La categoría con ID {vehiculo.CategoriaId} no existe.");
                }

                // Verificar que existe la sucursal
                var sucursal = _sucursalDAL.GetById(vehiculo.SucursalId);
                if (sucursal == null)
                {
                    throw new BusinessRuleException($"La sucursal con ID {vehiculo.SucursalId} no existe.");
                }

                // Verificar que no exista otro vehículo con la misma patente
                if (_vehiculoDAL.ExistsPatente(vehiculo.Patente, vehiculo.Id))
                {
                    throw new DuplicateEntityException($"Ya existe otro vehículo con patente {vehiculo.Patente}.");
                }

                bool result = _vehiculoDAL.Update(vehiculo);
                if (!result)
                {
                    throw new BusinessException("No se pudo actualizar el vehículo.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el vehículo.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var vehiculo = _vehiculoDAL.GetById(id);
                if (vehiculo == null)
                {
                    throw new EntityNotFoundException("Vehiculo", id);
                }

                // No permitir eliminar vehículos alquilados
                if (vehiculo.Estado == EstadoVehiculo.Alquilado)
                {
                    throw new BusinessRuleException("No se puede eliminar un vehículo que está actualmente alquilado.");
                }

                bool result = _vehiculoDAL.Delete(id);
                if (!result)
                {
                    throw new BusinessException("No se pudo eliminar el vehículo.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el vehículo con ID {id}.", ex);
            }
        }

        private void ValidarVehiculo(Vehiculo vehiculo)
        {
            // Validar Marca
            if (string.IsNullOrWhiteSpace(vehiculo.Marca))
            {
                throw new BusinessRuleException("La marca es obligatoria.");
            }

            if (vehiculo.Marca.Length > 50)
            {
                throw new BusinessRuleException("La marca no puede exceder 50 caracteres.");
            }

            // Validar Modelo
            if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
            {
                throw new BusinessRuleException("El modelo es obligatorio.");
            }

            if (vehiculo.Modelo.Length > 50)
            {
                throw new BusinessRuleException("El modelo no puede exceder 50 caracteres.");
            }

            // Validar Año
            int anioActual = DateTime.Now.Year;
            if (vehiculo.Anio < 1900 || vehiculo.Anio > anioActual + 1)
            {
                throw new BusinessRuleException($"El año debe estar entre 1900 y {anioActual + 1}.");
            }

            // Validar Patente (formato argentino: ABC123 o AB123CD)
            if (string.IsNullOrWhiteSpace(vehiculo.Patente))
            {
                throw new BusinessRuleException("La patente es obligatoria.");
            }

            vehiculo.Patente = vehiculo.Patente.ToUpper().Replace(" ", "").Replace("-", "");

            if (!Regex.IsMatch(vehiculo.Patente, @"^[A-Z]{3}\d{3}$") &&
                !Regex.IsMatch(vehiculo.Patente, @"^[A-Z]{2}\d{3}[A-Z]{2}$"))
            {
                throw new BusinessRuleException("El formato de la patente no es válido (ej: ABC123 o AB123CD).");
            }

            // Validar Color
            if (string.IsNullOrWhiteSpace(vehiculo.Color))
            {
                throw new BusinessRuleException("El color es obligatorio.");
            }

            // Validar Kilometraje
            if (vehiculo.Kilometraje < 0)
            {
                throw new BusinessRuleException("El kilometraje no puede ser negativo.");
            }

            if (vehiculo.Kilometraje > 1000000)
            {
                throw new BusinessRuleException("El kilometraje parece excesivo. Verifique el valor.");
            }

            // Validar IDs
            if (vehiculo.CategoriaId <= 0)
            {
                throw new BusinessRuleException("Debe seleccionar una categoría válida.");
            }

            if (vehiculo.SucursalId <= 0)
            {
                throw new BusinessRuleException("Debe seleccionar una sucursal válida.");
            }
        }
    }
}
