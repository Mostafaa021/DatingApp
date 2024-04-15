using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager ,
         ITokenService tokenService
         ,IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            
  
        }
        [HttpPost("register")] // api/account/register ==> in Body
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDTO registerDTO)
        {
            // Check if UserName exitsts or Not 
            if(await IsExisted(registerDTO.UserName)) return BadRequest("User is Taken");
             
            var user = _mapper.Map<RegisterDTO,AppUser>(registerDTO);

            //using var hmac = new HMACSHA512();

            user.UserName = registerDTO.UserName.ToLower();

            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            //user.PasswordSalt = hmac.Key;

            //_userManager.Users.Add(user); // will be commented as not used when using UserManager as Handled Automatically 
            //await _userManager.SaveChangesAsync();

            // instead of above two lines ==> assign register password of user to variable after saving it to database
            //CreateAsync(user, registerDTO.Password)  will create and save changes in database 
            var result =  await _userManager.CreateAsync(user, registerDTO.Password); 
            if (!result.Succeeded) return BadRequest(result.Errors);
            // after user register by default add user to role member 
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors); 
            
            return new UserDto
            {
                UserName = user.UserName,
                Token =  await _tokenService.CreateToken(user), // here we used await as Create Token works Asyncorounhs
                KnownAs = user.KnownAs,
                Gender = user.Gender

            }; 
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDTO loginDTO)
        {
            // Check That user name in Login already in Database or Not 
            var user = await _userManager.Users
            .Include(p=>p.Photos)
            .FirstOrDefaultAsync(x=>x.UserName == loginDTO.UserName);
            if(user == null) return Unauthorized("invalid User name");

            // Check password if valid CheckPasswordAsync using userManager
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) return Unauthorized("InValid Password");

           //// using hmac algorithms same as user hashing salt saved in database 
           // using var hmac = new HMACSHA512(user.PasswordSalt);
           //// hasing user login password with same Hash Algorithm
           // var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
           //// compare between bytes[] of user password login  and user password in database 
           // for (int i = 0; i < ComputedHash.Length; i++)
           // {
           //     if(ComputedHash[i] != user.PasswordHash[i]) return Unauthorized ("Invalid Password");
           // }
            return  new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.URL,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        //Function to Check User is Exist or Not
        private async Task<bool> IsExisted (string username)
        {
            return await _userManager.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}