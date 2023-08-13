using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(user=>user.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(like=>like.SourceUserId == likesParams.UserId);
                users = likes.Select(like=>like.TargetUser);
            }
            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like=>like.TargetUserId == likesParams.UserId);
                users = likes.Select(like=>like.SourceUser);
            }
            
            var likedUsers =  users.Select(user=>new LikeDto
            {
                UserName = user.UserName,
                Age = user.BirthDate.CalculateAge(),
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(p=>p.IsMain).URL,
                City = user.City,
                Id = user.Id,
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers,likesParams.PageSize, likesParams.PageNumber);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(l=>l.LikedUsers).FirstOrDefaultAsync(x=>x.Id == userId);
        }
    }
}