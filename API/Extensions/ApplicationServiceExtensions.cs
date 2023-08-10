using API.Data;
using API.Filters;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services ,
         IConfiguration config)
        {
        // Service to Connect with Database SQL Lite
        services.AddDbContext<DataContext>(options=>{ options.UseSqlite(config.GetConnectionString("MyConnection"));});
        // Cors Policy Service
        services.AddCors();
        // Inject per Scope
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IPhotoService,PhotoService>();
        // inject Filter as Service
        services.AddScoped<LogUserActivityFilter>();
        // inject service of AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
        // inject Cloudinary Services to uploadPhotos 
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        return services ;
        }   

        
    }
}