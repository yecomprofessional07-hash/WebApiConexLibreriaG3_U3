using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLbrosU3.Features.Inventario.Categorias.Dtos
{
    public class CategoriasAppService
    {
        private readonly LibreriaContext _context;
        public CategoriasAppService(LibreriaContext context) => _context = context;

        public async Task<List<CategoriaEntity>> ObtenerTodas() => await _context.Categorias.ToListAsync();

        public async Task<CategoriaEntity?> ObtenerPorId(int id) => await _context.Categorias.FindAsync(id);

        public async Task<ApiResponse<CategoriaEntity>> Guardar(CategoriaEntity cat)
        {
            cat.FechaCreacion = DateTime.Now;
            _context.Categorias.Add(cat);
            await _context.SaveChangesAsync();
            return new ApiResponse<CategoriaEntity>(cat);
        }

        public async Task<ApiResponse<CategoriaEntity>> Actualizar(CategoriaEntity cat)
        {
            _context.Categorias.Update(cat);
            await _context.SaveChangesAsync();

            return new ApiResponse<CategoriaEntity>(cat);
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return new ApiResponse<bool>(false, "La categoría no existe");
            }

            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Categoría eliminada exitosamente");
            }
            catch (Exception ex)
            {
                string errorReal = ex.InnerException?.Message ?? ex.Message;
                return new ApiResponse<bool>(false, $"No se puede eliminar: {errorReal}");
            }
        }
    }

}
