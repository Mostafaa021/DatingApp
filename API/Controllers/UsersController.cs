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
    }
}