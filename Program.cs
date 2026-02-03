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

var app = builder.Build();

// Habilitar Swagger en producción también (para que puedas probar la API)
app.UseSwagger();
app.UseSwaggerUI();

// Comentar esta línea para evitar problemas con HTTPS en Railway
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();