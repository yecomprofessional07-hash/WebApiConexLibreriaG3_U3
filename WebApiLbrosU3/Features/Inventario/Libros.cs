using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;


namespace WebApiLibrosU3.Features.Inventario.Libros
{
    public class LibrosAppService
    {
        private readonly LibreriaContext _context;
        public LibrosAppService(LibreriaContext context) => _context = context;

        public async Task<List<LibroEntity>> ObtenerTodos() => await _context.Libros.Include(l => l.Categoria).ToListAsync();

        public async Task<LibroEntity?> ObtenerPorId(int id) => await _context.Libros.FindAsync(id);

        public async Task<ApiResponse<LibroEntity>> Guardar(LibroEntity libro)
        {
            libro.FechaCreacion = DateTime.Now;
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return new ApiResponse<LibroEntity>(libro, "Guardado");
        }

        public async Task Actualizar(LibroEntity libro)
        {
            _context.Entry(libro).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }
        }
    }

}
