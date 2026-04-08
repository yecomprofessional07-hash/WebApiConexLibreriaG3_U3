using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLibrosU3.Features.Inventario.Categorias;

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriasAppService _categoriasService;

        public CategoriasController(CategoriasAppService categoriasService)
        {
            _categoriasService = categoriasService;
        }

        // READ: Listar todas
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoriaEntity>>>> GetAll()
        {
            var data = await _categoriasService.ObtenerTodas();
            return Ok(new ApiResponse<List<CategoriaEntity>>(data, "Lista de categorías cargada"));
        }

        // READ: Obtener por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoriaEntity>>> GetById(int id)
        {
            var categoria = await _categoriasService.ObtenerPorId(id);
            if (categoria == null)
                return NotFound(new ApiResponse<CategoriaEntity> { Success = false, Message = "Categoría no encontrada" });

            return Ok(new ApiResponse<CategoriaEntity>(categoria));
        }

        // CREATE: Guardar
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoriaEntity>>> Create([FromBody] CategoriaEntity categoria)
        {
            var response = await _categoriasService.Guardar(categoria);
            return Ok(response);
        }

        // UPDATE: Actualizar
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] CategoriaEntity categoria)
        {
            await _categoriasService.Actualizar(categoria);
            return Ok(new ApiResponse<bool>(true, "Categoría actualizada"));
        }

        // DELETE: Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _categoriasService.Eliminar(id);
            return Ok(new ApiResponse<bool>(true, "Categoría eliminada"));
        }
    }
}
