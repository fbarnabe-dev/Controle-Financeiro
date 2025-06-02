using System.Text;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services.AddDbContext<WFConFinDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var chave = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Chave").Get<string>());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

