using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // domain /api/users
    [Authorize]
    public class UsersController : BaseApiController
    {

       
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository , IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync(); 
            return Ok(users); // OK =>Return object of type Task<ActionResult>
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            return await _userRepository.GetMemberByIdAsync(id);
        }
        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<MemberDto>> GetUserByName(string name)
        {
             return await _userRepository.GetMemberByNameAsync(name);
        }
        [HttpPut]
        public async Task<ActionResult> EditUser(MemberUpdateDto memberUpdateDto)
        {
            var UserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUserNameAsync(UserName);
            if(user == null) return NotFound("User Not Found");
            _mapper.Map(memberUpdateDto , user); // just mapping before excuting update on database 
            try
            {
            if(await _userRepository.SaveAllAsync())
             return NoContent(); // 204 responce will be used with http put as no content to be returned
            return BadRequest("Failed to Update User");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the member: {ex.Message}");
            }
            
        }
    }
}