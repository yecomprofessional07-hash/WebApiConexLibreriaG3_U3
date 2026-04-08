using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;
using WebApiLbrosU3.Commons.Models;


namespace WebApiLbrosU3.Features.Facturacion.Ventas
{
    public class VentasAppService
    {
        private readonly LibreriaContext _context;

        public VentasAppService(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<VentaEntity>> ProcesarNuevaVenta(VentaEntity nuevaVenta)
        {
            var response = new ApiResponse<VentaEntity>();

            // Iniciamos una transacción para que si algo falla, no se guarde nada a medias
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Calcular el total y validar stock
                decimal total = 0;
                foreach (var detalle in nuevaVenta.Detalles)
                {
                    var libro = await _context.Libros.FindAsync(detalle.LibroId);
                    if (libro == null || libro.Stock < detalle.Cantidad)
                    {
                        response.Success = false;
                        response.Message = $"No hay stock suficiente para el libro: {libro?.Titulo ?? "Desconocido"}";
                        return response;
                    }

                    // Guardamos el precio actual en el detalle (lo que hablamos del reporte)
                    detalle.PrecioUnitario = libro.Precio;
                    total += (detalle.PrecioUnitario * detalle.Cantidad);

                    // 2. Restar stock
                    libro.Stock -= detalle.Cantidad;
                }

                nuevaVenta.TotalVenta = total;
                nuevaVenta.FechaVenta = DateTime.Now;

                // 3. Guardar en la base de datos
                // EF es tan listo que al guardar la Venta, guarda sus Detalles automáticamente
                _context.Ventas.Add(nuevaVenta);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                response.Success = true;
                response.Data = nuevaVenta;
                response.Message = "Venta realizada con éxito";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Success = false;
                response.Message = "Error al procesar venta: " + ex.Message;
            }

            return response;
        }
    }
}
