using System.Text.RegularExpressions;
using SistemaAlquilerAutos.DAL;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.BLL
{
    public class ClienteBLL
    {
        private readonly ClienteDAL _clienteDAL;

        public ClienteBLL()
        {
            _clienteDAL = new ClienteDAL();
        }

        public List<Cliente> GetAll()
        {
            try
            {
                return _clienteDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los clientes.", ex);
            }
        }

        public Cliente GetById(int id)
        {
            try
            {
                var cliente = _clienteDAL.GetById(id);
                if (cliente == null)
                {
                    throw new EntityNotFoundException("Cliente", id);
                }
                return cliente;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el cliente con ID {id}.", ex);
            }
        }

        public Cliente? GetByDNI(string dni)
        {
            try
            {
                return _clienteDAL.GetByDNI(dni);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al buscar cliente por DNI {dni}.", ex);
            }
        }

        public List<Cliente> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }
                return _clienteDAL.Search(searchText);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al buscar clientes.", ex);
            }
        }

        public int Insert(Cliente cliente)
        {
            try
            {
                // Validaciones de negocio
                ValidarCliente(cliente);

                // Verificar que no exista el DNI
                if (_clienteDAL.ExistsDNI(cliente.DNI))
                {
                    throw new DuplicateEntityException($"Ya existe un cliente con DNI {cliente.DNI}.");
                }

                return _clienteDAL.Insert(cliente);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al insertar el cliente.", ex);
            }
        }

        public void Update(Cliente cliente)
        {
            try
            {
                // Validaciones de negocio
                ValidarCliente(cliente);

                // Verificar que existe
                var existente = _clienteDAL.GetById(cliente.Id);
                if (existente == null)
                {
                    throw new EntityNotFoundException("Cliente", cliente.Id);
                }

                // Verificar que no exista otro cliente con el mismo DNI
                if (_clienteDAL.ExistsDNI(cliente.DNI, cliente.Id))
                {
                    throw new DuplicateEntityException($"Ya existe otro cliente con DNI {cliente.DNI}.");
                }

                bool result = _clienteDAL.Update(cliente);
                if (!result)
                {
                    throw new BusinessException("No se pudo actualizar el cliente.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el cliente.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var cliente = _clienteDAL.GetById(id);
                if (cliente == null)
                {
                    throw new EntityNotFoundException("Cliente", id);
                }

                bool result = _clienteDAL.Delete(id);
                if (!result)
                {
                    throw new BusinessException("No se pudo eliminar el cliente.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el cliente con ID {id}.", ex);
            }
        }

        private void ValidarCliente(Cliente cliente)
        {
            // Validar DNI
            if (string.IsNullOrWhiteSpace(cliente.DNI))
            {
                throw new BusinessRuleException("El DNI es obligatorio.");
            }

            if (!Regex.IsMatch(cliente.DNI, @"^\d{7,8}$"))
            {
                throw new BusinessRuleException("El DNI debe tener entre 7 y 8 dígitos.");
            }

            // Validar Nombre
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new BusinessRuleException("El nombre es obligatorio.");
            }

            if (cliente.Nombre.Length > 100)
            {
                throw new BusinessRuleException("El nombre no puede exceder 100 caracteres.");
            }

            // Validar Apellido
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
            {
                throw new BusinessRuleException("El apellido es obligatorio.");
            }

            if (cliente.Apellido.Length > 100)
            {
                throw new BusinessRuleException("El apellido no puede exceder 100 caracteres.");
            }

            // Validar Email
            if (!string.IsNullOrWhiteSpace(cliente.Email))
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(cliente.Email, emailRegex))
                {
                    throw new BusinessRuleException("El formato del email no es válido.");
                }
            }

            // Validar Teléfono
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                throw new BusinessRuleException("El teléfono es obligatorio.");
            }

            // Validar Edad (mayor de 18 años para alquilar)
            if (cliente.Edad < 18)
            {
                throw new BusinessRuleException("El cliente debe ser mayor de 18 años para alquilar vehículos.");
            }

            if (cliente.Edad > 120)
            {
                throw new BusinessRuleException("La fecha de nacimiento no es válida.");
            }

            // Validar Dirección
            if (string.IsNullOrWhiteSpace(cliente.Direccion))
            {
                throw new BusinessRuleException("La dirección es obligatoria.");
            }

            // Validar Ciudad
            if (string.IsNullOrWhiteSpace(cliente.Ciudad))
            {
                throw new BusinessRuleException("La ciudad es obligatoria.");
            }
        }
    }
}
