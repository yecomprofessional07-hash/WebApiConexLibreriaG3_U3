using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLbrosU3.Features.Inventario.Proveedores.Dtos
{
    public class ProveedoresAppService
    {
        private readonly LibreriaContext _context;
        public ProveedoresAppService(LibreriaContext context) => _context = context;

        // Mantenemos Entity porque el Controller usará esto para mapear a ProveedorDto
        public async Task<List<ProveedorEntity>> ObtenerTodos() =>
            await _context.Proveedores.ToListAsync();

        public async Task<ProveedorEntity?> ObtenerPorId(int id) =>
            await _context.Proveedores.FindAsync(id);

        // Adaptado para recibir la Entity que el Controller mapeó desde ProveedorCreateDto
        public async Task<ApiResponse<ProveedorEntity>> Guardar(ProveedorEntity prov)
        {
            prov.FechaCreacion = DateTime.Now;
            prov.Activo = true;

            _context.Proveedores.Add(prov);
            await _context.SaveChangesAsync();

            // Devolvemos la entidad para que el Controller pueda confirmar el ID creado
            return new ApiResponse<ProveedorEntity>(prov, "Proveedor Guardado con éxito");
        }

        // Cambiamos a Task<ApiResponse<bool>> para que el Controller sepa si el Update fue real
        public async Task<ApiResponse<bool>> Actualizar(ProveedorEntity prov)
        {
            try
            {
                _context.Proveedores.Update(prov);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Proveedor actualizado correctamente");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, "Error al actualizar: " + ex.Message);
            }
        }

        // Cambiamos a Task<ApiResponse<bool>> para manejar el error de FK (Libros vinculados)
        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            var prov = await _context.Proveedores.FindAsync(id);
            if (prov == null)
                return new ApiResponse<bool>(false, "El proveedor no existe");

            try
            {
                _context.Proveedores.Remove(prov);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Proveedor eliminado físicamente");
            }
            catch (Exception)
            {
                // Si el proveedor tiene filas en la tabla 'Libros', saltará aquí
                return new ApiResponse<bool>(false, "No se puede eliminar: existen libros registrados con este proveedor");
            }
        }
    }
}
