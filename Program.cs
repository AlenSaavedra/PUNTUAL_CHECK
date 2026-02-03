using API_PUNTUALCHECK.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar el puerto para Railway/nube
builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    options.ListenAnyIP(int.Parse(port));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext con MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

// Habilitar Swagger siempre (para testing en producción)
app.UseSwagger();
app.UseSwaggerUI();

// Comentar HTTPS redirect para Railway
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();