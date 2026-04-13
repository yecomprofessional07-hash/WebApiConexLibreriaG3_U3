namespace WebApiLbrosU3.Features.Inventario.Libros.Dtos
{
    public class LibroDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        // Esta es la parte clave:
        // En lugar de "public CategoriaEntity Categoria { get; set; }", usamos:
        public string CategoriaNombre { get; set; }
        public string ProveedorNombre { get; set; }
    }
}
