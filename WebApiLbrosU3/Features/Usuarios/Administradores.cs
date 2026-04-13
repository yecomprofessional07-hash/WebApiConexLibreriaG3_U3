using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Dtos;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLibrosU3.Features.Usuarios.Administradores
{
    public class AdministradoresAppService
    {
        private readonly LibreriaContext _context;
        public AdministradoresAppService(LibreriaContext context) => _context = context;

        public async Task<ApiResponse<AdministradorEntity>> LoginAdmin(LoginRequestDtos login)
        {
            //string nuevoHash = BCrypt.Net.BCrypt.HashPassword("pass123");
            //Console.WriteLine($"NUEVO HASH PARA SQL: {nuevoHash}");
            var admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Correo == login.Correo);

            if (admin == null) return new ApiResponse<AdministradorEntity>(null!, "Correo no existe") { Success = false };

            
            bool coincide = BCrypt.Net.BCrypt.Verify(login.Password.Trim(), admin.Contraseña.Trim());

            if (!coincide)
            {
                return new ApiResponse<AdministradorEntity>(null!, "Error: La contraseña es incorrecta") { Success = false };
            }

            return new ApiResponse<AdministradorEntity>(admin, "Bienvenido Administrador");
        }

    }
}
