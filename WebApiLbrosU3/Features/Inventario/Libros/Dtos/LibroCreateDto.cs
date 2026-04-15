using System.ComponentModel.DataAnnotations;

namespace WebApiLbrosU3.Features.Inventario.Libros.Dtos
{
    namespace WebApiLbrosU3.Features.Inventario.Libros.DTOs
    {
        public class LibroCreateDto
        {
            [Required(ErrorMessage = "El título es obligatorio")]
            [StringLength(150)]
            public string Titulo { get; set; } = string.Empty;

            public string? Autor { get; set; }

            [Range(0.01, 9999.99, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal Precio { get; set; }

            [Range(0, int.MaxValue)]
            public int Stock { get; set; }

            [Required(ErrorMessage = "La sinopsis es obligatoria")]
            [StringLength(255)]
            public string Sinopsis { get; set; } = string.Empty;

            // Aquí recibes el ID para la llave foránea de SQL
            [Required]
            public int CategoriaId { get; set; }

            [Required]
            public int ProveedorId { get; set; }
        }
    }

}
