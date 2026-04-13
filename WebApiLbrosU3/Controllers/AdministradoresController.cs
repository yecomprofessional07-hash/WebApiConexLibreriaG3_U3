using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Dtos;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLibrosU3.Features.Usuarios.Administradores;

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradoresController : ControllerBase
    {
        private readonly AdministradoresAppService _adminService;

        public AdministradoresController(AdministradoresAppService adminService)
        {
            _adminService = adminService;
        }

        // Usamos POST para login porque enviamos datos sensibles en el cuerpo de la petición
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> LoginAdmin([FromBody] LoginRequestDtos login)
        {
            // 1. Llamamos al servicio de administración
            var response = await _adminService.LoginAdmin(login);

            if (!response.Success || response.Data == null)
            {
                return Unauthorized(new ApiResponse<object>(null!, response.Message));
            }

            // 2. Devolvemos solo lo necesario (Nombre e Id)
            // Usamos 'object' o un AdministradorDto para no enviar la contraseña
            var adminData = new
            {
                Id = response.Data.Id,
                Nombre = response.Data.Nombre
            };

            return Ok(new ApiResponse<object>(adminData, "Bienvenido al panel de administración"));
        }

    }
}
