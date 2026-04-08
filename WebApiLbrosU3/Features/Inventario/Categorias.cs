using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLibrosU3.Features.Inventario.Categorias
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

        public async Task Actualizar(CategoriaEntity cat)
        {
            _context.Categorias.Update(cat);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat != null) { _context.Categorias.Remove(cat); await _context.SaveChangesAsync(); }
        }
    }

}
