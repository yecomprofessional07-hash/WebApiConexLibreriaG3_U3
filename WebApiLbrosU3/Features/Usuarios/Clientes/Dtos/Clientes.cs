using BCrypt.Net; // Asegúrate de tener instalado BCrypt.Net-Next
using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;
using WebApiLbrosU3.Commons.Dtos;

namespace WebApiLbrosU3.Features.Usuarios.Clientes.Dtos
{
    public class ClientesAppService
    {
        private readonly LibreriaContext _context;
        public ClientesAppService(LibreriaContext context) => _context = context;

        public async Task<List<ClienteEntity>> ObtenerTodos() => await _context.Clientes.ToListAsync();

        public async Task<ClienteEntity?> ObtenerPorId(int id) => await _context.Clientes.FindAsync(id);

        public async Task<ApiResponse<ClienteEntity>> Guardar(ClienteEntity cliente)
        {
            // 1. Validar si el DNI ya existe (Regla de negocio)
            var existe = await _context.Clientes.AnyAsync(c => c.DNI == cliente.DNI);
            if (existe) return new ApiResponse<ClienteEntity>(null!, "El DNI ya está registrado");

            // 2. CIFRADO DE CONTRASEÑA (Vital)
            cliente.Contraseña = BCrypt.Net.BCrypt.HashPassword(cliente.Contraseña);

            cliente.FechaCreacion = DateTime.Now;
            cliente.Activo = true;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return new ApiResponse<ClienteEntity>(cliente, "Cliente guardado exitosamente");
        }

        public async Task<ApiResponse<bool>> Actualizar(ClienteEntity cliente)
        {
            try
            {
                // Buscamos el cliente original para no perder datos que no vienen del Front
                var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);
                if (clienteExistente == null) return new ApiResponse<bool>(false, "Cliente no encontrado");

                // Actualizamos solo campos permitidos (No tocamos la contraseña ni fecha de creación)
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.DNI = cliente.DNI;
                clienteExistente.Correo = cliente.Correo;

                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Cliente actualizado con éxito");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, "Error al actualizar: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return new ApiResponse<bool>(false, "El cliente no existe");

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Cliente eliminado exitosamente");
            }
            catch (Exception)
            {
                // Bloqueado por FK si tiene ventas
                return new ApiResponse<bool>(false, "No se puede eliminar: el cliente tiene historial de ventas");
            }
        }
        public async Task<ApiResponse<ClienteEntity>> Autenticar(LoginRequestDtos login)
        {
            string nuevoHash = BCrypt.Net.BCrypt.HashPassword("pass123");
            Console.WriteLine($"NUEVO HASH PARA SQL: {nuevoHash} pass123");
            // Buscamos por correo
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == login.Correo);

            // Verificamos existencia y contraseña con BCrypt
            if (cliente == null) return new ApiResponse<ClienteEntity>(null!, "Correo no existe") { Success = false };


            bool coincide = BCrypt.Net.BCrypt.Verify(login.Password.Trim(), cliente.Contraseña.Trim());

            if (!coincide)
            {
                return new ApiResponse<ClienteEntity>(null!, "Error: Credenciales de cliente incorrectas") { Success = false };
            }

            return new ApiResponse<ClienteEntity>(cliente, "Bienvenido usuario");
        }
    }
}
