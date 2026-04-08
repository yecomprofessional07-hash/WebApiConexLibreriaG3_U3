namespace WebApiLbrosU3.Enitities
{
    public class ProveedorEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        // Navegación
        public List<LibroEntity> Libros { get; set; } = new();
    }

}
