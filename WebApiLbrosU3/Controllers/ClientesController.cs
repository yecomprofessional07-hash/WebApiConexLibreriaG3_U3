using Microsoft.AspNetCore.Mvc;
using WebApiLbrosU3.Commons.Dtos;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Features.Usuarios.Clientes.Dtos;
using WebApiLbrosU3.Features.Usuarios.Clientes.Dtos.WebApiLbrosU3.Features.Usuarios.Clientes.Dtos;

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

        // READ: Obtener todos (USANDO DTO PARA OCULTAR CONTRASEÑA)
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ClientesDtos>>>> GetAll()
        {
            var entities = await _clientesService.ObtenerTodos();

            // Mapeamos a ClienteDto para que la contraseña nunca salga de la API
            var dtos = entities.Select(c => new ClientesDtos
            {
                Id = c.Id,
                Nombre = c.Nombre,
                DNI = c.DNI,
                Correo = c.Correo!
            }).ToList();

            return Ok(new ApiResponse<List<ClientesDtos>>(dtos, "Lista de clientes cargada"));
        }

        // READ: Obtener por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClientesDtos>>> GetById(int id)
        {
            var c = await _clientesService.ObtenerPorId(id);
            if (c == null)
                return NotFound(new ApiResponse<ClientesDtos> { Success = false, Message = "Cliente no encontrado" });
            // Mapeamos a DTO por seguridad
            var dto = new ClientesDtos { Id = c.Id, Nombre = c.Nombre, DNI = c.DNI, Correo = c.Correo! };

            return Ok(new ApiResponse<ClientesDtos>(dto));
        }

        // CREATE: Guardar (USANDO CREATE DTO Y PREPARANDO PARA CIFRADO)
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClienteEntity>>> Create([FromBody] ClienteCreateDtos dto)
        {
            // Mapeamos el DTO a la Entidad
            var nuevaEntidad = new ClienteEntity
            {
                Nombre = dto.Nombre,
                DNI = dto.DNI,
                Correo = dto.Correo,
                Contraseña = dto.Contraseña // El Service se encargará de cifrarla
            };

            var response = await _clientesService.Guardar(nuevaEntidad);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        // UPDATE: Actualizar con validación de ID
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] ClienteEntity cliente)
        {
            if (id != cliente.Id)
                return BadRequest(new ApiResponse<bool>(false, "El ID no coincide"));

            var response = await _clientesService.Actualizar(cliente);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        // DELETE: Eliminar con validación de respuesta
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _clientesService.Eliminar(id);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        // LOGIN: Autenticar cliente y devolver DTO sin contraseña
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<ClientesDtos>>> Login([FromBody] LoginRequestDtos login)
        {
            // 1. Llamamos al servicio para autenticar
            var response = await _clientesService.Autenticar(login);

            if (!response.Success || response.Data == null)
            {
                // Devolvemos 401 Unauthorized si las credenciales fallan
                return Unauthorized(new ApiResponse<ClientesDtos>(null!, response.Message));
            }

            // 2. Mapeamos la Entity a un DTO de respuesta (Seguridad)
            var clienteDto = new ClientesDtos
            {
                Id = response.Data.Id,
                Nombre = response.Data.Nombre,
                Correo = response.Data.Correo ?? string.Empty,
                DNI = response.Data.DNI
            };

            return Ok(new ApiResponse<ClientesDtos>(clienteDto, "Login exitoso"));
        }

    }
}
