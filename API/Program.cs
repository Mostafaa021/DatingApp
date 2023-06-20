using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Custom Extension methods extend IServiceCollection  whihc Contain some services To make Code Clean and Readable
builder.Services.AddApplicationServices(builder.Configuration); 
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); // Allowing CORS Policy
app.UseHttpsRedirection(); // redirecting from HTTP to HTTPs
app.UseAuthentication(); // Ask you Have a Valid Token With Logged Name and Password ?
app.UseAuthorization(); // Ask if Your Token Has Role on Some Actions allowed to do  ?
app.MapControllers();

app.Run();
