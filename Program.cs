using Microsoft.EntityFrameworkCore;
using PRACTICA2.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var options = new DbContextOptionsBuilder<DataContext>()
    .UseSqlite("Data Source= proyecto.db")
    .Options;



builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlite("Data Source = proyecto.db"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
