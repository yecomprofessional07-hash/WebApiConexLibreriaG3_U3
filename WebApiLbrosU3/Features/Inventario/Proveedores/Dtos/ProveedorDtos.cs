namespace WebApiLbrosU3.Features.Inventario.Proveedores.Dtos
{
    public class ProveedorDtos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
    }
}
