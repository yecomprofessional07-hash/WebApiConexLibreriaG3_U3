namespace WebApiLbrosU3.Enitities
{
    public class VentaEntity
    {
        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }

        public int ClienteId { get; set; }
        // Navegación
        public ClienteEntity Cliente { get; set; } = null!;
        public List<VentaDetallesEntity> Detalles { get; set; } = new();
    }

}
