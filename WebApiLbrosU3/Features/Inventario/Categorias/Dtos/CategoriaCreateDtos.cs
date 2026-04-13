using System.ComponentModel.DataAnnotations;

namespace WebApiLbrosU3.Features.Inventario.Categorias.Dtos
{
    public class CategoriaCreateDtos
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}
