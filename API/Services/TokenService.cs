using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // declare private key 
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        
        public string CreateToken(AppUser user) // Returning String as Token will be Hashed using Security Key to String 
        {
            // Claims to be Signed to JWT PayLoad
            var claims = new List<Claim>
            {
                // using Registered Claims => 3 types of Claims (Registered-Public-Private)
                new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName , user.UserName)
                
            };
           // Create Signing Credential wiht Private Key and Hashing Algorithm
           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

           // Descripe Token 
           var TokenDescriptor = new SecurityTokenDescriptor
           {
             SigningCredentials = creds ,
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(10)
           };
            // Create instance from Token Handler 
            var TokenHandler = new JwtSecurityTokenHandler();
            // use object instantiated to create token 
            var token = TokenHandler.CreateToken(TokenDescriptor);
            //Write Token as String like JWT.IO
            return TokenHandler.WriteToken(token);


        }
    }
}