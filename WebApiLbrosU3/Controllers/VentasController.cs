using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Features.Facturacion.Dtos;
// Importante para VentaCreateDto

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly VentasAppService _ventasService;

        public VentasController(VentasAppService ventasService)
        {
            _ventasService = ventasService;
        }

        // CREATE: Procesar una nueva venta usando DTO
        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> Post([FromBody] VentaCreateDtos ventaDto)
        {
            // 1. Validación de negocio básica
            if (ventaDto.Detalles == null || !ventaDto.Detalles.Any())
            {
                return BadRequest(new ApiResponse<bool>(false, "La venta debe contener al menos un libro."));
            }

            // 2. Llamamos al servicio pasando el DTO
            var response = await _ventasService.ProcesarNuevaVenta(ventaDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // READ: Historial de ventas (Sugerencia de implementación con DTO)
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VentaDtos>>>> Get()
        {
            // Aquí llamarías a un método del service que use .Select() para mapear a VentaDto
            // Por ahora mantenemos la respuesta informativa pero con el tipo correcto
            return Ok(new ApiResponse<string>("Endpoint de historial preparado para devolver VentaDto"));
        }
    }
}
