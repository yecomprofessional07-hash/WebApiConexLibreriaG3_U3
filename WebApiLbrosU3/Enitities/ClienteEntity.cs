namespace WebApiLbrosU3.Enitities
{
    public class ClienteEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string DNI { get; set; } = null!;
        public string? Correo { get; set; }
        public string Contraseña { get; set; } = "";
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        // Navegación
        public List<VentaEntity> Ventas { get; set; } = new();
    }

}
