using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;

namespace WebApiLibrosU3.Features.Usuarios.Administradores
{
    public class AdministradoresAppService
    {
        private readonly LibreriaContext _context;

        public AdministradoresAppService(LibreriaContext context)
        {
            _context = context;
        }

        // Método para el Login simple
        public async Task<ApiResponse<AdministradorEntity>> Login(string nombre, string password)
        {
            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Nombre == nombre && a.Contraseña == password);

            if (admin == null)
                return new ApiResponse<AdministradorEntity> { Success = false, Message = "Usuario o contraseña incorrectos" };

            return new ApiResponse<AdministradorEntity>(admin, "Login exitoso");
        }
    }
}
