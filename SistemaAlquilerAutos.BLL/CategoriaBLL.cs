using SistemaAlquilerAutos.DAL;
using SistemaAlquilerAutos.Entity;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.BLL
{
    public class CategoriaBLL
    {
        private readonly CategoriaDAL _categoriaDAL;

        public CategoriaBLL()
        {
            _categoriaDAL = new CategoriaDAL();
        }

        public List<Categoria> GetAll()
        {
            try
            {
                return _categoriaDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener las categorías.", ex);
            }
        }

        public Categoria GetById(int id)
        {
            try
            {
                var categoria = _categoriaDAL.GetById(id);
                if (categoria == null)
                {
                    throw new EntityNotFoundException("Categoria", id);
                }
                return categoria;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener la categoría con ID {id}.", ex);
            }
        }

        public int Insert(Categoria categoria)
        {
            try
            {
                // Validaciones de negocio
                ValidarCategoria(categoria);

                return _categoriaDAL.Insert(categoria);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al insertar la categoría.", ex);
            }
        }

        public void Update(Categoria categoria)
        {
            try
            {
                // Validaciones de negocio
                ValidarCategoria(categoria);

                // Verificar que existe
                var existente = _categoriaDAL.GetById(categoria.Id);
                if (existente == null)
                {
                    throw new EntityNotFoundException("Categoria", categoria.Id);
                }

                bool result = _categoriaDAL.Update(categoria);
                if (!result)
                {
                    throw new BusinessException("No se pudo actualizar la categoría.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la categoría.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var categoria = _categoriaDAL.GetById(id);
                if (categoria == null)
                {
                    throw new EntityNotFoundException("Categoria", id);
                }

                bool result = _categoriaDAL.Delete(id);
                if (!result)
                {
                    throw new BusinessException("No se pudo eliminar la categoría.");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar la categoría con ID {id}.", ex);
            }
        }

        private void ValidarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                throw new BusinessRuleException("El nombre de la categoría es obligatorio.");
            }

            if (categoria.Nombre.Length > 100)
            {
                throw new BusinessRuleException("El nombre de la categoría no puede exceder 100 caracteres.");
            }

            if (categoria.PrecioDiario <= 0)
            {
                throw new BusinessRuleException("El precio diario debe ser mayor a cero.");
            }

            if (categoria.PrecioDiario > 1000000)
            {
                throw new BusinessRuleException("El precio diario no puede exceder $1.000.000.");
            }
        }
    }
}
