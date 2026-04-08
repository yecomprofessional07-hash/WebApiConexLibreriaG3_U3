using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLibrosU3.Features.Inventario.Proveedores
{
    public class ProveedoresAppService
    {
        private readonly LibreriaContext _context;
        public ProveedoresAppService(LibreriaContext context) => _context = context;

        public async Task<List<ProveedorEntity>> ObtenerTodos() => await _context.Proveedores.ToListAsync();

        public async Task<ProveedorEntity?> ObtenerPorId(int id) => await _context.Proveedores.FindAsync(id);

        public async Task<ApiResponse<ProveedorEntity>> Guardar(ProveedorEntity prov)
        {
            prov.FechaCreacion = DateTime.Now; 
            _context.Proveedores.Add(prov);
            await _context.SaveChangesAsync();

            return new ApiResponse<ProveedorEntity>(prov, "Proveedor Guardado");
        }

        public async Task Actualizar(ProveedorEntity prov) { _context.Proveedores.Update(prov); await _context.SaveChangesAsync(); }

        public async Task Eliminar(int id)
        {
            var prov = await _context.Proveedores.FindAsync(id);
            if (prov != null) { _context.Proveedores.Remove(prov); await _context.SaveChangesAsync(); }
        }
    }

}
