using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ApiResponse<AdministradorEntity>>> Login([FromBody] AdministradorEntity loginData)
        {
            // Validamos que el cliente envíe los datos
            if (string.IsNullOrEmpty(loginData.Nombre) || string.IsNullOrEmpty(loginData.Contraseña))
            {
                return BadRequest(new ApiResponse<AdministradorEntity>
                {
                    Success = false,
                    Message = "Nombre y contraseña son requeridos"
                });
            }

            var response = await _adminService.Login(loginData.Nombre, loginData.Contraseña);

            if (!response.Success)
            {
                // Si las credenciales fallan, devolvemos 401 Unauthorized
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}
