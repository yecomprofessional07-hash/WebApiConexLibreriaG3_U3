namespace WebApiLbrosU3.Features.Inventario.Libros.Dtos
{
    public class LibroDto
    {
        public int Id { get; set; }
        // 'required' obliga a que se asigne al crear el objeto, solucionando el aviso
        public required string Titulo { get; set; }
        public required string Autor { get; set; }

        public decimal Precio { get; set; }
        public int Stock { get; set; }

        // Para campos que podrían ser opcionales, usa string?
        public string? Sinopsis { get; set; }

        // Para las propiedades de aplanamiento (Flattening)
        public required string CategoriaNombre { get; set; }
        public required string ProveedorNombre { get; set; }
    }
}
