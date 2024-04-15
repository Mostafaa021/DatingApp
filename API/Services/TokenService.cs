using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // declare private key 
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config ,
            UserManager<AppUser> userManager )
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
        }
        
        public  async Task<string> CreateToken(AppUser user) // Returning String as Token will be Hashed using Security Key to String 
        {
            // Claims to be Signed to JWT PayLoad
            var claims = new List<Claim>
            {
                // using Registered Claims => 3 types of Claims (Registered-Public-Private)
                new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName , user.UserName)
                
            };

            // get roles of user
            var roles = await _userManager.GetRolesAsync(user);
            // add to previous claims roles , but not all roles just name of role so we used select to projection  
            claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role , role)));
           // Create Signing Credential wiht Private Key and Hashing Algorithm
           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

           // Description of Token 
           var TokenDescriptor = new SecurityTokenDescriptor
           {
             SigningCredentials = creds ,
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(5)
           };
            // Create instance from Token Handler 
            var TokenHandler = new JwtSecurityTokenHandler();
            // use object instantiated to create token of type security token 
            var token = TokenHandler.CreateToken(TokenDescriptor);
            //Write Token as String like JWT.IO
             var tokenAsString =  TokenHandler.WriteToken(token);

            return tokenAsString;

        }
    }
}