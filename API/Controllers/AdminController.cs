using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseApiController

    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUserWithRoles() // here will get all users beside thier roles 
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new                            // project users as user id , username , and roles of username
                {
                    u.Id,
                    userName = u.UserName,
                    Roles = u.UserRoles.Select(r =>r.Role.Name).ToList(),   
                    
                }).ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> EditUserRoles( string username , [FromQuery] string roles)
        {
            // check if roles is empty from querystring 
            if (string.IsNullOrEmpty(roles)) return BadRequest("Should have one role at least");
            // get user FindByNameAsync by name 
            var user =  await _userManager.FindByNameAsync(username);

            // if user not exists return not found error 
            if (user == null) return NotFound("User Not Found");

            // get current roles for user
            var userRoles = await _userManager.GetRolesAsync(user);

            // roles that i want to edit and add to this user <> split roles by (,) then put it in array ,
            var selectedRoles = roles.Split(",").ToArray();

            // if user already has no role
            if (userRoles == null) return BadRequest("user already has no role");

            // add to user selected roles exept his current role
           var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded) return BadRequest("Failed to add role");

            // if i wanna to remove role from user 
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove role");

            var FinaluserRoles = await _userManager.GetRolesAsync(user);

            return Ok(FinaluserRoles);
        }

        [HttpGet("photos-to-moderate")]
        [Authorize(Policy = "RequireModeratePhotoRole")]
        public ActionResult GetPhotoForModeration()
        {
            return Ok("Only Admins or  Moderators Can See This");
        }
    }
}
