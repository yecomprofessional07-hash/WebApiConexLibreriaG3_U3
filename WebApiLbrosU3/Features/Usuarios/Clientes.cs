using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLibrosU3.Features.Usuarios.Clientes
{
    public class ClientesAppService
    {
        private readonly LibreriaContext _context;
        public ClientesAppService(LibreriaContext context) => _context = context;

        public async Task<List<ClienteEntity>> ObtenerTodos() => await _context.Clientes.ToListAsync();

        public async Task<ApiResponse<ClienteEntity>> Guardar(ClienteEntity cliente)
        {
            cliente.FechaCreacion = DateTime.Now;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return new ApiResponse<ClienteEntity>(cliente, "Cliente guardado");
        }

        public async Task Actualizar(ClienteEntity cli) { _context.Clientes.Update(cli); await _context.SaveChangesAsync(); }

        public async Task<ClienteEntity?> ObtenerPorId(int id) => await _context.Clientes.FindAsync(id);

        public async Task Eliminar(int id)
        {
            var cli = await _context.Clientes.FindAsync(id);
            if (cli != null) { _context.Clientes.Remove(cli); await _context.SaveChangesAsync(); }
        }
    }

}
