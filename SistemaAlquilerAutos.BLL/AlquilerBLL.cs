using SistemaAlquilerAutos.DAL;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.BLL
{
    public class AlquilerBLL
    {
        private readonly AlquilerDAL _alquilerDAL;
        private readonly ClienteDAL _clienteDAL;
        private readonly VehiculoDAL _vehiculoDAL;
        private readonly SucursalDAL _sucursalDAL;
        private readonly CategoriaDAL _categoriaDAL;

        public AlquilerBLL()
        {
            _alquilerDAL = new AlquilerDAL();
            _clienteDAL = new ClienteDAL();
            _vehiculoDAL = new VehiculoDAL();
            _sucursalDAL = new SucursalDAL();
            _categoriaDAL = new CategoriaDAL();
        }

        public List<Alquiler> GetAll()
        {
            try
            {
                return _alquilerDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los alquileres.", ex);
            }
        }

        public List<Alquiler> GetActivos()
        {
            try
            {
                return _alquilerDAL.GetActivos();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los alquileres activos.", ex);
            }
        }

        public List<Alquiler> GetByCliente(int clienteId)
        {
            try
            {
                return _alquilerDAL.GetByCliente(clienteId);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener los alquileres del cliente {clienteId}.", ex);
            }
        }

        public Alquiler GetById(int id)
        {
            try
            {
                var alquiler = _alquilerDAL.GetById(id);
                if (alquiler == null)
                {
                    throw new EntityNotFoundException("Alquiler", id);
                }
                return alquiler;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el alquiler con ID {id}.", ex);
            }
        }

        /// <summary>
        /// Calcula el precio total del alquiler basado en la categoría del vehículo y los días
        /// </summary>
        public decimal CalcularPrecioAlquiler(int categoriaId, int dias)
        {
            try
            {
                if (dias <= 0)
                {
                    throw new BusinessRuleException("La cantidad de días debe ser mayor a cero.");
                }

                var categoria = _categoriaDAL.GetById(categoriaId);
                if (categoria == null)
                {
                    throw new EntityNotFoundException("Categoria", categoriaId);
                }

                decimal precioBase = categoria.PrecioDiario * dias;

                // Aplicar descuentos por volumen
                // 5% de descuento para alquileres de 7 días o más
                if (dias >= 7 && dias < 15)
                {
                    precioBase *= 0.95m;
                }
                // 10% de descuento para alquileres de 15 días o más
                else if (dias >= 15 && dias < 30)
                {
                    precioBase *= 0.90m;
                }
                // 15% de descuento para alquileres de 30 días o más
                else if (dias >= 30)
                {
                    precioBase *= 0.85m;
                }

                return Math.Round(precioBase, 2);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al calcular el precio del alquiler.", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo alquiler con todas las validaciones y transacciones
        /// </summary>
        public int CrearAlquiler(Alquiler alquiler)
        {
            using (var transaction = new TransactionManager())
            {
                try
                {
                    // Validaciones básicas
                    ValidarAlquiler(alquiler);

                    // Verificar que el cliente existe
                    var cliente = _clienteDAL.GetById(alquiler.ClienteId);
                    if (cliente == null)
                    {
                        throw new EntityNotFoundException("Cliente", alquiler.ClienteId);
                    }

                    if (!cliente.Activo)
                    {
                        throw new BusinessRuleException("El cliente no está activo.");
                    }

                    // Verificar que el vehículo existe y está disponible
                    var vehiculo = _vehiculoDAL.GetById(alquiler.VehiculoId);
                    if (vehiculo == null)
                    {
                        throw new EntityNotFoundException("Vehiculo", alquiler.VehiculoId);
                    }

                    if (vehiculo.Estado != EstadoVehiculo.Disponible)
                    {
                        throw new BusinessRuleException($"El vehículo {vehiculo.Patente} no está disponible para alquiler.");
                    }

                    // Verificar que no haya un alquiler activo para este vehículo
                    var alquilerActivo = _alquilerDAL.GetAlquilerActivoByVehiculo(alquiler.VehiculoId);
                    if (alquilerActivo != null)
                    {
                        throw new BusinessRuleException($"El vehículo ya tiene un alquiler activo (ID: {alquilerActivo.Id}).");
                    }

                    // Verificar que la sucursal existe
                    var sucursal = _sucursalDAL.GetById(alquiler.SucursalRetiroId);
                    if (sucursal == null)
                    {
                        throw new EntityNotFoundException("Sucursal", alquiler.SucursalRetiroId);
                    }

                    // Calcular precio si no está establecido
                    if (alquiler.PrecioTotal <= 0)
                    {
                        int dias = (alquiler.FechaDevolucionPrevista - alquiler.FechaInicio).Days;
                        alquiler.PrecioTotal = CalcularPrecioAlquiler(vehiculo.CategoriaId, dias);
                    }

                    // Establecer el kilometraje inicial del vehículo
                    alquiler.KilometrajeInicio = vehiculo.Kilometraje;

                    // Insertar el alquiler
                    int alquilerId = _alquilerDAL.Insert(alquiler, transaction);

                    // Actualizar el estado del vehículo a Alquilado
                    bool vehiculoActualizado = _vehiculoDAL.UpdateEstado(
                        alquiler.VehiculoId,
                        EstadoVehiculo.Alquilado,
                        transaction
                    );

                    if (!vehiculoActualizado)
                    {
                        throw new BusinessException("No se pudo actualizar el estado del vehículo.");
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    return alquilerId;
                }
                catch (BusinessException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BusinessException("Error al crear el alquiler.", ex);
                }
            }
        }

        /// <summary>
        /// Finaliza un alquiler (devolución del vehículo)
        /// </summary>
        public void FinalizarAlquiler(int alquilerId, DateTime fechaDevolucion, int kilometrajeFin,
                                      int? sucursalDevolucionId, string? observaciones)
        {
            using (var transaction = new TransactionManager())
            {
                try
                {
                    // Obtener el alquiler
                    var alquiler = _alquilerDAL.GetById(alquilerId);
                    if (alquiler == null)
                    {
                        throw new EntityNotFoundException("Alquiler", alquilerId);
                    }

                    if (alquiler.Estado != EstadoAlquiler.Activo)
                    {
                        throw new BusinessRuleException("El alquiler no está activo.");
                    }

                    // Validaciones
                    if (fechaDevolucion < alquiler.FechaInicio)
                    {
                        throw new BusinessRuleException("La fecha de devolución no puede ser anterior a la fecha de inicio.");
                    }

                    if (kilometrajeFin < alquiler.KilometrajeInicio)
                    {
                        throw new BusinessRuleException("El kilometraje final no puede ser menor al inicial.");
                    }

                    if (sucursalDevolucionId.HasValue)
                    {
                        var sucursal = _sucursalDAL.GetById(sucursalDevolucionId.Value);
                        if (sucursal == null)
                        {
                            throw new EntityNotFoundException("Sucursal", sucursalDevolucionId.Value);
                        }
                    }

                    // Finalizar el alquiler
                    bool alquilerFinalizado = _alquilerDAL.FinalizarAlquiler(
                        alquilerId,
                        fechaDevolucion,
                        kilometrajeFin,
                        sucursalDevolucionId,
                        observaciones,
                        transaction
                    );

                    if (!alquilerFinalizado)
                    {
                        throw new BusinessException("No se pudo finalizar el alquiler.");
                    }

                    // Actualizar el kilometraje y estado del vehículo
                    var vehiculo = _vehiculoDAL.GetById(alquiler.VehiculoId);
                    if (vehiculo != null)
                    {
                        vehiculo.Kilometraje = kilometrajeFin;
                        vehiculo.Estado = EstadoVehiculo.Disponible;

                        // Si se devolvió en otra sucursal, actualizar la sucursal del vehículo
                        if (sucursalDevolucionId.HasValue && sucursalDevolucionId.Value != alquiler.SucursalRetiroId)
                        {
                            vehiculo.SucursalId = sucursalDevolucionId.Value;
                        }

                        bool vehiculoActualizado = _vehiculoDAL.Update(vehiculo);
                        if (!vehiculoActualizado)
                        {
                            throw new BusinessException("No se pudo actualizar el vehículo.");
                        }
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                }
                catch (BusinessException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BusinessException("Error al finalizar el alquiler.", ex);
                }
            }
        }

        /// <summary>
        /// Cancela un alquiler
        /// </summary>
        public void CancelarAlquiler(int alquilerId, string motivoCancelacion)
        {
            using (var transaction = new TransactionManager())
            {
                try
                {
                    // Obtener el alquiler
                    var alquiler = _alquilerDAL.GetById(alquilerId);
                    if (alquiler == null)
                    {
                        throw new EntityNotFoundException("Alquiler", alquilerId);
                    }

                    if (alquiler.Estado != EstadoAlquiler.Activo)
                    {
                        throw new BusinessRuleException("Solo se pueden cancelar alquileres activos.");
                    }

                    if (string.IsNullOrWhiteSpace(motivoCancelacion))
                    {
                        throw new BusinessRuleException("Debe indicar el motivo de cancelación.");
                    }

                    // Cancelar el alquiler
                    bool alquilerCancelado = _alquilerDAL.CancelarAlquiler(
                        alquilerId,
                        motivoCancelacion,
                        transaction
                    );

                    if (!alquilerCancelado)
                    {
                        throw new BusinessException("No se pudo cancelar el alquiler.");
                    }

                    // Volver a poner el vehículo como disponible
                    bool vehiculoActualizado = _vehiculoDAL.UpdateEstado(
                        alquiler.VehiculoId,
                        EstadoVehiculo.Disponible,
                        transaction
                    );

                    if (!vehiculoActualizado)
                    {
                        throw new BusinessException("No se pudo actualizar el estado del vehículo.");
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                }
                catch (BusinessException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BusinessException("Error al cancelar el alquiler.", ex);
                }
            }
        }

        private void ValidarAlquiler(Alquiler alquiler)
        {
            if (alquiler.FechaInicio < DateTime.Today)
            {
                throw new BusinessRuleException("La fecha de inicio no puede ser anterior a hoy.");
            }

            if (alquiler.FechaDevolucionPrevista <= alquiler.FechaInicio)
            {
                throw new BusinessRuleException("La fecha de devolución debe ser posterior a la fecha de inicio.");
            }

            int diasAlquiler = (alquiler.FechaDevolucionPrevista - alquiler.FechaInicio).Days;
            if (diasAlquiler > 365)
            {
                throw new BusinessRuleException("No se pueden realizar alquileres mayores a 365 días.");
            }

            if (alquiler.ClienteId <= 0)
            {
                throw new BusinessRuleException("Debe seleccionar un cliente.");
            }

            if (alquiler.VehiculoId <= 0)
            {
                throw new BusinessRuleException("Debe seleccionar un vehículo.");
            }

            if (alquiler.SucursalRetiroId <= 0)
            {
                throw new BusinessRuleException("Debe seleccionar una sucursal de retiro.");
            }
        }
    }
}
