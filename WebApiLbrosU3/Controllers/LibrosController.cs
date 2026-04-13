using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Features.Inventario.Libros.Dtos;
using WebApiLbrosU3.Features.Inventario.Libros.Dtos.WebApiLbrosU3.Features.Inventario.Libros.DTOs;

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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<LibroDto>>>> GetAll()
        {
            var librosEntity = await _librosService.ObtenerTodos();

            var dataDto = librosEntity.Select(l => new LibroDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Autor = l.Autor,
                Precio = l.Precio,
                Stock = l.Stock,
                CategoriaNombre = l.Categoria?.Nombre ?? "Sin Categoría",
                ProveedorNombre = l.Proveedor?.Nombre ?? "Sin Proveedor"
            }).ToList();

            return Ok(new ApiResponse<List<LibroDto>>(dataDto, "Catálogo de libros cargado"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LibroEntity>>> GetById(int id)
        {
            var libro = await _librosService.ObtenerPorId(id);
            if (libro == null)
                return NotFound(new ApiResponse<LibroEntity> { Success = false, Message = "Libro no encontrado" });

            return Ok(new ApiResponse<LibroEntity>(libro));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LibroEntity>>> Create([FromBody] LibroCreateDto dto)
        {
            var nuevaEntidad = new LibroEntity
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId,
                ProveedorId = dto.ProveedorId,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            var response = await _librosService.Guardar(nuevaEntidad);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // CORRECCIÓN: Usar el ID en la URL y validar la respuesta del servicio
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] LibroEntity libro)
        {
            // Validación de seguridad básica
            if (id != libro.Id)
                return BadRequest(new ApiResponse<bool>(false, "El ID de la URL no coincide con el del cuerpo"));

            var response = await _librosService.Actualizar(libro);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // CORRECCIÓN: Validar la respuesta del servicio (importante por FK)
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _librosService.Eliminar(id);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
