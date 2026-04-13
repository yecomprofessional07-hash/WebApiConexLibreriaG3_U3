using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApiLbrosU3.Features.Usuarios.Clientes.Dtos
{
    public class ClientesDtos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string DNI { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }
}
