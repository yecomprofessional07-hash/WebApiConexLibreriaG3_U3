using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Features.Facturacion.Ventas;

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

        // CREATE: Procesar una nueva venta (Maestro-Detalle)
        [HttpPost]
        public async Task<ActionResult<ApiResponse<VentaEntity>>> Post([FromBody] VentaEntity nuevaVenta)
        {
            // Validamos que la venta traiga al menos un producto
            if (nuevaVenta.Detalles == null || nuevaVenta.Detalles.Count == 0)
            {
                return BadRequest(new ApiResponse<VentaEntity>
                {
                    Success = false,
                    Message = "No se puede realizar una venta sin productos."
                });
            }

            var response = await _ventasService.ProcesarNuevaVenta(nuevaVenta);

            if (!response.Success)
            {
                // Si falló por falta de stock u otro motivo de negocio
                return BadRequest(response);
            }

            return Ok(response);
        }

        // READ: Opcional - Historial de ventas para reportes
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VentaEntity>>>> Get()
        {
            // Este método puedes implementarlo en tu Service después si te queda tiempo
            // Por ahora, es útil para verificar que se guardaron las ventas.
            return Ok(new ApiResponse<string>("Historial de ventas listo para implementar"));
        }
    }
}
