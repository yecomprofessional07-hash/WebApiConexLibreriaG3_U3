namespace WebApiLbrosU3.Enitities
{
    public class LibroEntity
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Autor { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int CategoriaId { get; set; }
        public int ProveedorId { get; set; }
        // Navegación
        public CategoriaEntity? Categoria { get; set; } = null!;
        public ProveedorEntity? Proveedor { get; set; } = null!;
    }

}
