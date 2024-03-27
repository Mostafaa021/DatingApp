using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServicesExtensions
    {
        
        public static IServiceCollection AddIdentityServices(this IServiceCollection services ,
         IConfiguration config) 
        {
            //Authentication for JWT Service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(options=>{
            options.RequireHttpsMetadata = true;
            options.SaveToken = true ;
            options.TokenValidationParameters = new TokenValidationParameters
            {
            ValidateIssuerSigningKey = true , // Validate Token  based on Signing key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])), // Declare SigningKey 
            ValidateIssuer = false,
            ValidateAudience = false 
            };
        }); 
        return services ;
        }
    }
}