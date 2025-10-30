namespace SistemaAlquilerAutos.BLL.Exceptions
{
    /// <summary>
    /// Excepción base para todas las excepciones de negocio
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException() : base()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción cuando una regla de negocio no se cumple
    /// </summary>
    public class BusinessRuleException : BusinessException
    {
        public BusinessRuleException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción cuando una entidad no se encuentra
    /// </summary>
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(string entityName, object id)
            : base($"{entityName} con ID {id} no encontrado.")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción cuando ya existe una entidad duplicada
    /// </summary>
    public class DuplicateEntityException : BusinessException
    {
        public DuplicateEntityException(string message) : base(message)
        {
        }
    }
}
