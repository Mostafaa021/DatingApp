using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context ,
         ITokenService tokenService
         ,IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;
  
        }
        [HttpPost("register")] // api/account/register ==> in Body
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDTO registerDTO)
        {
            // Check if UserName exitsts or Not 
            if(await IsExisted(registerDTO.UserName)) return BadRequest("User is Taken");
            
            var user = _mapper.Map<RegisterDTO,AppUser>(registerDTO);

            using var hmac = new HMACSHA512();

            user.UserName = registerDTO.UserName.ToLower();
            
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            }; 
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDTO loginDTO)
        {
            // Check That user name in Login already in Database or Not 
            var user = await _context.Users
            .Include(p=>p.Photos)
            .FirstOrDefaultAsync(x=>x.UserName == loginDTO.UserName);
            if(user == null) return Unauthorized("invalid User name");
            // using hmac algorithms same as user hashing salt saved in database 
            using var hmac = new HMACSHA512(user.PasswordSalt);
           // hasing user login password with same Hash Algorithm
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            // compare between bytes[] of user password login  and user password in database 
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if(ComputedHash[i] != user.PasswordHash[i]) return Unauthorized ("Invalid Password");
            }
            return  new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.URL,
                KnownAs = user.KnownAs
            };
        }
        //Function to Check User is Exist or Not
        private async Task<bool> IsExisted (string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}