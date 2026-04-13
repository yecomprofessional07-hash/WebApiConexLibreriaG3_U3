using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;


namespace WebApiLbrosU3.Features.Inventario.Libros.Dtos
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

        public async Task<ApiResponse<LibroEntity>> Actualizar(LibroEntity Libro)
        {
            _context.Libros.Update(Libro);
            await _context.SaveChangesAsync();

            return new ApiResponse<LibroEntity>(Libro);
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            var libros = await _context.Libros.FindAsync(id);

            if (libros == null)
            {
                return new ApiResponse<bool>(false, "El libro no existe");
            }

            try
            {
                _context.Libros.Remove(libros);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "libro eliminado exitosamente");
            }
            catch (Exception ex)
            {
                string errorReal = ex.InnerException?.Message ?? ex.Message;
                return new ApiResponse<bool>(false, $"No se puede eliminar: {errorReal}");
            }
        }
    }

}
