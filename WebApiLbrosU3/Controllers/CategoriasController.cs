using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Features.Inventario.Categorias.Dtos;

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
        public async Task<ActionResult<ApiResponse<List<CategoriaDtos>>>> GetAll()
        {
            var entities = await _categoriasService.ObtenerTodas();

            // Mapeamos a DTO
            var dtos = entities.Select(c => new CategoriaDtos
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();

            return Ok(new ApiResponse<List<CategoriaDtos>>(dtos));
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
        public async Task<ActionResult<ApiResponse<bool>>> Create([FromBody] CategoriaCreateDtos categoria)
        {
            // 1. Mapeo: Convertimos el DTO en la Entity de base de datos
            var nuevaCategoria = new CategoriaEntity
            {
                Nombre = categoria.Nombre,
                // Asignamos valores automáticos que no vienen del usuario
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            // 2. Llamamos al servicio con la Entity
            var response = await _categoriasService.Guardar(nuevaCategoria);

            // 3. Verificamos si hubo error en el servicio
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // UPDATE: Actualizar

        [HttpPut("{id}")] // Agregamos el ID en la URL por estándar REST
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] CategoriaDtos dto)
        {
            // 1. Validación de seguridad: ¿El ID de la URL coincide con el del cuerpo?
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponse<bool>(false, "El ID de la categoría no coincide"));
            }

            // 2. Mapeo: Convertimos el DTO a la Entity
            // Aquí solo mapeamos lo que permitimos editar (el nombre)
            var categoriaExistente = new CategoriaEntity
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                // Normalmente mantenemos el estado Activo o lo recibimos del DTO si es necesario
                Activo = true
            };

            // 3. Ejecución
            var response = await _categoriasService.Actualizar(categoriaExistente);

            if (!response.Success)
                return BadRequest(response);

            return Ok(new ApiResponse<bool>(true, "Categoría actualizada exitosamente"));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            // 1. Ejecutamos la eliminación y guardamos el resultado del servicio
            var response = await _categoriasService.Eliminar(id);

            // 2. Si el servicio falló (ej. el ID no existe), devolvemos BadRequest o NotFound
            if (!response.Success)
            {
                return BadRequest(response); // Aquí va el mensaje "No se encontró la categoría"
            }

            // 3. Si todo salió bien, devolvemos el éxito
            return Ok(response);
        }
    }
}
