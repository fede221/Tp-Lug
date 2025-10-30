using System.Text.RegularExpressions;
using SistemaAlquilerAutos.DAL;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.BLL
{
    public class SucursalBLL
    {
        private readonly SucursalDAL _sucursalDAL;

        public SucursalBLL()
        {
            _sucursalDAL = new SucursalDAL();
        }

        public List<Sucursal> GetAll()
        {
            try
            {
                return _sucursalDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener las sucursales.", ex);
            }
        }

        public Sucursal GetById(int id)
        {
            try
            {
                var sucursal = _sucursalDAL.GetById(id);
                if (sucursal == null)
                {
                    throw new EntityNotFoundException("Sucursal", id);
                }
                return sucursal;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la sucursal con ID {id}.", ex);
            }
        }

        public List<Sucursal> GetByCity(string ciudad)
        {
            try
            {
                return _sucursalDAL.GetByCity(ciudad);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener sucursales de {ciudad}.", ex);
            }
        }

        public int Insert(Sucursal sucursal)
        {
            try
            {
                // Validaciones de negocio
                ValidarSucursal(sucursal);

                return _sucursalDAL.Insert(sucursal);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al insertar la sucursal.", ex);
            }
        }

        public void Update(Sucursal sucursal)
        {
            try
            {
                // Validaciones de negocio
                ValidarSucursal(sucursal);

                // Verificar que existe
                var existente = _sucursalDAL.GetById(sucursal.Id);
                if (existente == null)
                {
                    throw new EntityNotFoundException("Sucursal", sucursal.Id);
                }

                bool result = _sucursalDAL.Update(sucursal);
                if (!result)
                {
                    throw new BusinessException("No se pudo actualizar la sucursal.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la sucursal.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var sucursal = _sucursalDAL.GetById(id);
                if (sucursal == null)
                {
                    throw new EntityNotFoundException("Sucursal", id);
                }

                bool result = _sucursalDAL.Delete(id);
                if (!result)
                {
                    throw new BusinessException("No se pudo eliminar la sucursal.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar la sucursal con ID {id}.", ex);
            }
        }

        private void ValidarSucursal(Sucursal sucursal)
        {
            // Validar Nombre
            if (string.IsNullOrWhiteSpace(sucursal.Nombre))
            {
                throw new BusinessRuleException("El nombre de la sucursal es obligatorio.");
            }

            if (sucursal.Nombre.Length > 100)
            {
                throw new BusinessRuleException("El nombre no puede exceder 100 caracteres.");
            }

            // Validar Dirección
            if (string.IsNullOrWhiteSpace(sucursal.Direccion))
            {
                throw new BusinessRuleException("La dirección es obligatoria.");
            }

            if (sucursal.Direccion.Length > 200)
            {
                throw new BusinessRuleException("La dirección no puede exceder 200 caracteres.");
            }

            // Validar Ciudad
            if (string.IsNullOrWhiteSpace(sucursal.Ciudad))
            {
                throw new BusinessRuleException("La ciudad es obligatoria.");
            }

            if (sucursal.Ciudad.Length > 100)
            {
                throw new BusinessRuleException("La ciudad no puede exceder 100 caracteres.");
            }

            // Validar Teléfono
            if (string.IsNullOrWhiteSpace(sucursal.Telefono))
            {
                throw new BusinessRuleException("El teléfono es obligatorio.");
            }

            // Validar Email
            if (!string.IsNullOrWhiteSpace(sucursal.Email))
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(sucursal.Email, emailRegex))
                {
                    throw new BusinessRuleException("El formato del email no es válido.");
                }
            }
        }
    }
}
