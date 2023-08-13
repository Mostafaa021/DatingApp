using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using API.Helpers;

namespace API.Controllers
{

    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likeRepository;
        
        public LikesController(IUserRepository userRepository , ILikesRepository likeRepository)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            
        }
        [HttpPost("{username:alpha}")]
        public async Task<IActionResult> AddLike(string username)
        {
            if(User.Identity.IsAuthenticated)
            {
                var sourceUserId = User.GetUserId();
                var likedUser = await _userRepository.GetUserByUserNameAsync(username);
                var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

                if(likedUser == null) return NotFound();

                if(sourceUser.UserName == username) return BadRequest("can`t like yourself");

                var UserLike = await _likeRepository.GetUserLike(sourceUserId , likedUser.Id);

                if(UserLike != null) return BadRequest("you Already liked this user") ;

                UserLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = likedUser.Id
                };
                sourceUser.LikedUsers.Add(UserLike);

                if(await _userRepository.SaveAllAsync()) return Ok();
                return BadRequest("failed to Like User");

            }
            return BadRequest();
        }
        [HttpGet]
        public async Task <ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
          likesParams.UserId = User.GetUserId();
         var users = await _likeRepository.GetUserLikes(likesParams);
          Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize ,
          users.RecordsCount , users.PagesCount));
         
         return Ok(users);
        }
    }
}