namespace WebApiLbrosU3.Enitities
{
    public class VentaDetallesEntity
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int VentaId { get; set; }
        public int LibroId { get; set; }
        // Navegación
        public VentaEntity Venta { get; set; } = null!;
        public LibroEntity Libro { get; set; } = null!;
    }

}
