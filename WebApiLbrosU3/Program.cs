using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Infrastructure.Data; // Tu carpeta del Contexto
using WebApiLibrosU3.Features.Inventario.Libros;
using WebApiLibrosU3.Features.Inventario.Categorias;
using WebApiLibrosU3.Features.Inventario.Proveedores;
using WebApiLibrosU3.Features.Usuarios.Clientes;
using WebApiLibrosU3.Features.Usuarios.Administradores;
using WebApiLbrosU3.Features.Facturacion.Ventas;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DEL CONTEXTO (SQL SERVER)
builder.Services.AddDbContext<LibreriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibreriaDBConnectionString")));

// 2. REGISTRO DE SERVICIOS (Dependency Injection)
// Inventario
builder.Services.AddScoped<LibrosAppService>();
builder.Services.AddScoped<CategoriasAppService>();
builder.Services.AddScoped<ProveedoresAppService>();

// Usuarios
builder.Services.AddScoped<ClientesAppService>();
builder.Services.AddScoped<AdministradoresAppService>();

// Facturación
builder.Services.AddScoped<VentasAppService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// 3. CONFIGURACIÓN DE SWAGGER (Para probar tu API)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();