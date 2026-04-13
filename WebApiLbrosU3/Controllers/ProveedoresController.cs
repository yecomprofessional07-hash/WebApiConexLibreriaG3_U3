using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Features.Inventario.Proveedores.Dtos;

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

        // READ: Listar todos usando ProveedorDto
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProveedorDtos>>>> GetAll()
        {
            var entities = await _proveedoresService.ObtenerTodos();

            // Mapeamos a DTO para enviar solo lo necesario a Angular
            var dtos = entities.Select(p => new ProveedorDtos
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Telefono = p.Telefono,
                Correo = p.Correo
            }).ToList();

            return Ok(new ApiResponse<List<ProveedorDtos>>(dtos, "Lista de proveedores cargada"));
        }

        // READ: Obtener por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProveedorEntity>>> GetById(int id)
        {
            var proveedor = await _proveedoresService.ObtenerPorId(id);
            if (proveedor == null)
                return NotFound(new ApiResponse<ProveedorEntity> { Success = false, Message = "Proveedor no encontrado" });

            return Ok(new ApiResponse<ProveedorEntity>(proveedor));
        }

        // CREATE: Recibe ProveedorCreateDto y mapea a Entity
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProveedorEntity>>> Create([FromBody] ProveedorCreateDtos dto)
        {
            var nuevaEntidad = new ProveedorEntity
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Activo = true
                // La FechaCreacion la asigna el Service
            };

            var response = await _proveedoresService.Guardar(nuevaEntidad);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // UPDATE: Ahora valida la respuesta del Service
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] ProveedorEntity proveedor)
        {
            if (id != proveedor.Id)
                return BadRequest(new ApiResponse<bool>(false, "El ID no coincide"));

            var response = await _proveedoresService.Actualizar(proveedor);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // DELETE: Ahora informa si no se pudo eliminar por FK (Libros asociados)
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _proveedoresService.Eliminar(id);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
