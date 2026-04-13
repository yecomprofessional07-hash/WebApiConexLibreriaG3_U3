namespace WebApiLbrosU3.Features.Facturacion.Dtos
{
    public class VentaDtos
    {
        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }

        // Información aplanada
        public string NombreCliente { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }

        public List<VentaDetalleDto> Detalles { get; set; } = new();

    }
    public class VentaDetalleDto
    {
        public string TituloLibro { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
