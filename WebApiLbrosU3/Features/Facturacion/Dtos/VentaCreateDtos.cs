using System.ComponentModel.DataAnnotations;

namespace WebApiLbrosU3.Features.Facturacion.Dtos
{
    public class VentaCreateDtos
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un producto")]
        public List<VentaDetalleCreateDto> Detalles { get; set; } = new();
    }

    public class VentaDetalleCreateDto
    {
        [Required]
        public int LibroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        // El precio se suele enviar desde el front para registro histórico, 
        // aunque el servidor debería validarlo.
        public decimal PrecioUnitario { get; set; }
    }
}
