using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options=>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MyConnection")); 
    
});
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); // Allowing CORS Policy
app.UseHttpsRedirection(); // redirecting from HTTP to HTTPs

app.MapControllers();

app.Run();
