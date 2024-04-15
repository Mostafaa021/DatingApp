using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.MiddleWares;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Identity;
using API.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddNewtonsoftJson(options=>options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);// To Stop Object Cycle 
//Custom Extension methods extend IServiceCollection  whihc Contain some services To make Code Clean and Readable
builder.Services.AddApplicationServices(builder.Configuration); 
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleWare>(); // Use Custom MiddleWare which Logging Error
// if (app.Environment.IsDevelopment())
// {
//    app.UseDeveloperExceptionPage();
// }
app.UseHttpsRedirection(); // redirecting from HTTP to HTTPs
app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200", "http://localhost:4200")); // Allowing CORS Policy
app.UseAuthentication(); // Ask you Have a Valid Token With Logged Name and Password ?
app.UseAuthorization(); // Ask if Your Token Has Role on Some Actions allowed to do  ?
app.MapControllers();

using var scope = app.Services.CreateScope(); // this step to make scope access to above services 
var services = scope.ServiceProvider; // access above services inside created Scope
try
{
 var context =services.GetRequiredService<DataContext>(); // get context which will be seed inside
 var userManager = services.GetRequiredService<UserManager<AppUser>>(); // get UserManager  instead of DataContext Used
 var roleManager = services.GetRequiredService<RoleManager<AppRole>>(); // get RoleManager  instead of DataContext Used
 await context.Database.MigrateAsync(); // this function to ensure migration and if dropped database will create new one with seeded data
 await Seed.SeedUsers(userManager , roleManager); // Then seed Data in database
}
catch (Exception ex)
{
 var logger  = services.GetService<ILogger<Program>>();  // in case of failure 
 logger.LogError(ex , "Something Wrong Occured During Migration");
}
app.Run();
