using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Enitities;

namespace WebApiLbrosU3.Infrastructure.Data
{
    public class LibreriaContext : DbContext
    {
        public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options) { }

        // Definición de las Tablas (DbSets)
        public DbSet<CategoriaEntity> Categorias { get; set; }
        public DbSet<ProveedorEntity> Proveedores { get; set; }
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<LibroEntity> Libros { get; set; }
        public DbSet<AdministradorEntity> Administradores { get; set; }
        public DbSet<VentaEntity> Ventas { get; set; }
        public DbSet<VentaDetallesEntity> VentaDetalles { get; set; }

        // Configuración de reglas especiales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configurar precisión para el dinero (Decimales)
            modelBuilder.Entity<LibroEntity>().Property(p => p.Precio).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<VentaEntity>().Property(p => p.TotalVenta).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<VentaDetallesEntity>().Property(p => p.PrecioUnitario).HasColumnType("decimal(10,2)");

            // 2. Mapeo manual de nombres de tablas (por si acaso el plural falla)
            modelBuilder.Entity<AdministradorEntity>().ToTable("Administrador");
            modelBuilder.Entity<VentaDetallesEntity>().ToTable("VentaDetalles");

            // 3. Opcional: Configuraciones de borrado en cascada o valores por defecto
            // Por ejemplo, para que la fecha se ponga sola si no la envías:
            modelBuilder.Entity<LibroEntity>().Property(p => p.FechaCreacion).HasDefaultValueSql("GETDATE()");
        }
    }
}
