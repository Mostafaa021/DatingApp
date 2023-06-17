using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options=>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MyConnection")); 
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseHttpsRedirection(); // redirecting from HTTP to HTTPs

app.MapControllers();

app.Run();
