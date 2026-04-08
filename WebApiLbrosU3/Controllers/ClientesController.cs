using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLibrosU3.Features.Usuarios.Clientes;

namespace WebApiLibrosU3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesAppService _clientesService;

        public ClientesController(ClientesAppService clientesService)
        {
            _clientesService = clientesService;
        }

        // READ: Obtener todos
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ClienteEntity>>>> GetAll()
        {
            var data = await _clientesService.ObtenerTodos();
            return Ok(new ApiResponse<List<ClienteEntity>>(data, "Lista de clientes cargada"));
        }

        // READ: Obtener por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClienteEntity>>> GetById(int id)
        {
            var cliente = await _clientesService.ObtenerPorId(id);
            if (cliente == null)
                return NotFound(new ApiResponse<ClienteEntity> { Success = false, Message = "Cliente no encontrado" });

            return Ok(new ApiResponse<ClienteEntity>(cliente));
        }

        // CREATE: Guardar
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClienteEntity>>> Create([FromBody] ClienteEntity cliente)
        {
            var response = await _clientesService.Guardar(cliente);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        // UPDATE: Actualizar
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] ClienteEntity cliente)
        {
            await _clientesService.Actualizar(cliente);
            return Ok(new ApiResponse<bool>(true, "Cliente actualizado con éxito"));
        }

        // DELETE: Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _clientesService.Eliminar(id);
            return Ok(new ApiResponse<bool>(true, "Cliente eliminado físicamente de la base de datos"));
        }
    }
}
