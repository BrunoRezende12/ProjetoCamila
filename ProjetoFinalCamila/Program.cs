using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços
builder.Services.AddControllers();

// Configurando o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Supabase CRUD API",
        Version = "v1"
    });
});

// Registrando HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Usando o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Supabase CRUD API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
