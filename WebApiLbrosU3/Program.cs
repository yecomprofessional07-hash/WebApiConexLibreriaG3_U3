using Microsoft.EntityFrameworkCore;
using WebApiLbrosU3.Infrastructure.Data;
using WebApiLibrosU3.Features.Usuarios.Administradores;
using WebApiLbrosU3.Features.Inventario.Libros.Dtos;
using WebApiLbrosU3.Features.Inventario.Categorias.Dtos;
using WebApiLbrosU3.Features.Inventario.Proveedores.Dtos;
using WebApiLbrosU3.Features.Facturacion.Dtos;
using WebApiLbrosU3.Features.Usuarios.Clientes.Dtos;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DEL CONTEXTO (SQL SERVER)
builder.Services.AddDbContext<LibreriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibreriaDBConnectionString")));

// --- CORRECCIÓN CORS: DEFINICIÓN DE POLÍTICA ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") // Permite tu app de Angular
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// 2. REGISTRO DE SERVICIOS (Dependency Injection)
builder.Services.AddScoped<LibrosAppService>();
builder.Services.AddScoped<CategoriasAppService>();
builder.Services.AddScoped<ProveedoresAppService>();
builder.Services.AddScoped<ClientesAppService>();
builder.Services.AddScoped<AdministradoresAppService>();
builder.Services.AddScoped<VentasAppService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita el bucle infinito Venta -> Detalle -> Venta
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 3. CONFIGURACIÓN DE SWAGGER
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// --- CORRECCIÓN CORS: ACTIVACIÓN ---
// Importante: Debe ir antes de MapControllers y después de HttpsRedirection
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
