using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Enitities;
using WebApiLbrosU3.Infrastructure.Data;
using WebApiLbrosU3.Commons.Models;
using WebApiLbrosU3.Features.Facturacion.Dtos; // Asegúrate de que este sea el namespace de tus DTOs

namespace WebApiLbrosU3.Features.Facturacion.Dtos
{
    public class VentasAppService
    {
        private readonly LibreriaContext _context;

        public VentasAppService(LibreriaContext context)
        {
            _context = context;
        }

        // Ahora recibimos el DTO de creación
        public async Task<ApiResponse<bool>> ProcesarNuevaVenta(VentaCreateDtos ventaDto)
        {
            var response = new ApiResponse<bool>();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Creamos la entidad principal de la venta
                var nuevaVenta = new VentaEntity
                {
                    ClienteId = ventaDto.ClienteId,
                    FechaVenta = DateTime.Now,
                    TotalVenta = 0, // Lo calcularemos abajo
                    Detalles = new List<VentaDetallesEntity>()
                };

                decimal totalAcumulado = 0;

                // 2. Procesamos cada detalle del DTO
                foreach (var detalleDto in ventaDto.Detalles)
                {
                    // Buscamos el libro para validar existencia, stock y obtener precio real
                    var libro = await _context.Libros.FindAsync(detalleDto.LibroId);

                    if (libro == null)
                    {
                        response.Success = false;
                        response.Message = $"El libro con ID {detalleDto.LibroId} no existe.";
                        return response;
                    }

                    if (libro.Stock < detalleDto.Cantidad)
                    {
                        response.Success = false;
                        response.Message = $"Stock insuficiente para: {libro.Titulo}. Disponible: {libro.Stock}";
                        return response;
                    }

                    // 3. Creamos la entidad detalle
                    var detalleEntity = new VentaDetallesEntity
                    {
                        LibroId = libro.Id,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = libro.Precio // Usamos el precio real de la DB, no el del front
                    };

                    totalAcumulado += (detalleEntity.PrecioUnitario * detalleEntity.Cantidad);

                    // 4. Descontamos el stock
                    libro.Stock -= detalleDto.Cantidad;

                    // Agregamos el detalle a la lista de la venta
                    nuevaVenta.Detalles.Add(detalleEntity);
                }

                nuevaVenta.TotalVenta = totalAcumulado;

                // 5. Guardar todo el bloque
                _context.Ventas.Add(nuevaVenta);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Venta realizada con éxito";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Success = false;
                response.Message = "Error crítico al procesar venta: " + ex.Message;
            }

            return response;
        }
    }
}
