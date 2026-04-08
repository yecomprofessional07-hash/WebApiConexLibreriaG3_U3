using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLibrosU3.Features.Inventario.Libros;

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly LibrosAppService _librosService;

        public LibrosController(LibrosAppService librosService)
        {
            _librosService = librosService;
        }

        // READ: Obtener todos (incluye Categoría y Proveedor)
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<LibroEntity>>>> GetAll()
        {
            var data = await _librosService.ObtenerTodos();
            return Ok(new ApiResponse<List<LibroEntity>>(data, "Catálogo de libros cargado"));
        }

        // READ: Obtener por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LibroEntity>>> GetById(int id)
        {
            var libro = await _librosService.ObtenerPorId(id);
            if (libro == null)
                return NotFound(new ApiResponse<LibroEntity> { Success = false, Message = "Libro no encontrado" });

            return Ok(new ApiResponse<LibroEntity>(libro));
        }

        // CREATE: Guardar
        [HttpPost]
        public async Task<ActionResult<ApiResponse<LibroEntity>>> Create([FromBody] LibroEntity libro)
        {
            var response = await _librosService.Guardar(libro);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        // UPDATE: Actualizar
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] LibroEntity libro)
        {
            await _librosService.Actualizar(libro);
            return Ok(new ApiResponse<bool>(true, "Información del libro actualizada"));
        }

        // DELETE: Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _librosService.Eliminar(id);
            return Ok(new ApiResponse<bool>(true, "Libro eliminado del inventario"));
        }
    }
}
