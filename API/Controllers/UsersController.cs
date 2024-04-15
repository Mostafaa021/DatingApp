using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Helpers;

namespace API.Controllers
{
    // domain/api/users
    [Authorize] // as we already making authorize attribute to check authorization 
    public class UsersController : BaseApiController
    {

       
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository ,
         IMapper mapper,
         IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {   //* get current logged in user
            var CurrentUser = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            userParams.CurrentUserName =  CurrentUser.UserName;
           
            // here to project oposite of user in gender if female => male | male=> female
            if(string.IsNullOrEmpty(userParams.Gender))
            {
             userParams.Gender = CurrentUser.Gender == "male"?"female":"male" ;
            }
            
            var users = await _userRepository.GetMembersAsync(userParams); 
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize ,
             users.RecordsCount , users.PagesCount));
            return Ok(users); // OK =>Return object of type Task<ActionResult>
        }
       
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            return await _userRepository.GetMemberByIdAsync(id);
        }

       
        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<MemberDto>> GetUserByName(string name)
        {
             if(await _userRepository.GetMemberByNameAsync(name) == null)
             {
                return NotFound("Not Founded");
             }
             return await _userRepository.GetMemberByNameAsync(name);
            
        }

        [HttpPut]
        public async Task<ActionResult> EditUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            if(user == null) return NotFound("User Not Found");

           var Result =  await _photoService.UploadPhotoAsync(file);

           if(Result.Error != null) return BadRequest(Result.Error.Message);

           var Photo = new Photo {
            URL = Result.SecureUrl.AbsoluteUri , 
            PublicId = Result.PublicId,
           };

           if(user.Photos.Count ==0) Photo.IsMain = true;
           user.Photos.Add(Photo);
           if(await _userRepository.SaveAllAsync()) 
           {
              return CreatedAtAction(nameof(GetUserByName), 
              new {name = user.UserName},_mapper.Map<PhotoDto>(Photo)); 
              // the best practice to return from Post method 201 Responce directed to 
              //another action with location of added resource
           }
            return BadRequest("Error During Adding Photo ");
        }

        [HttpPut("set-main-photo/{PhotoId}")]
        public async Task<ActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername()); 
            if(user == null) return NotFound("User Not Exist"); 
            var photo = user.Photos.FirstOrDefault(x=>x.Id == PhotoId);
            if(photo == null) return NotFound("User don`t have Photos");

            if(photo.IsMain) return BadRequest("Selected Photo already Main Photo"); 

            var CurrentMainPhoto = user.Photos.FirstOrDefault(x=>x.IsMain);
            if(CurrentMainPhoto != null) CurrentMainPhoto.IsMain = false ; 
            photo.IsMain=true;

            if(await _userRepository.SaveAllAsync()) return NoContent(); 

            return BadRequest("Problem During Setting Main Photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(photo=>photo.Id == photoId);
            if(photo == null) return NotFound("Photo Not Found");
            if(photo.IsMain) return BadRequest ("Can`t Delete Your Main Photo"); 
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest (result.Error.Message);
            }

            user.Photos.Remove(photo); 
            if(await _userRepository.SaveAllAsync()) 
            return Ok();
            else
            return BadRequest("Something abnormal happened");
        }

    }
}