using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLibrosU3.Features.Inventario.Proveedores;

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedoresAppService _proveedoresService;

        public ProveedoresController(ProveedoresAppService proveedoresService)
        {
            _proveedoresService = proveedoresService;
        }

        // READ: Listar todos
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProveedorEntity>>>> GetAll()
        {
            var data = await _proveedoresService.ObtenerTodos();
            return Ok(new ApiResponse<List<ProveedorEntity>>(data, "Lista de proveedores cargada"));
        }

        // READ: Obtener por Id (Requerido para Inventario)
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProveedorEntity>>> GetById(int id)
        {
            var proveedor = await _proveedoresService.ObtenerPorId(id);
            if (proveedor == null)
                return NotFound(new ApiResponse<ProveedorEntity> { Success = false, Message = "Proveedor no encontrado" });

            return Ok(new ApiResponse<ProveedorEntity>(proveedor));
        }

        // CREATE: Guardar nuevo proveedor
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProveedorEntity>>> Create([FromBody] ProveedorEntity proveedor)
        {
            // Nota: Asegúrate que tu Service devuelva ApiResponse para que esta línea funcione
            var response = await _proveedoresService.Guardar(proveedor);
            return Ok(response);
        }

        // UPDATE: Actualizar información
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] ProveedorEntity proveedor)
        {
            await _proveedoresService.Actualizar(proveedor);
            return Ok(new ApiResponse<bool>(true, "Proveedor actualizado correctamente"));
        }

        // DELETE: Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _proveedoresService.Eliminar(id);
            return Ok(new ApiResponse<bool>(true, "Proveedor eliminado del sistema"));
        }
    }
}
