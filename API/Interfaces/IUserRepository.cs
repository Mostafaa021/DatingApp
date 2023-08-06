using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user) ;
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetAppUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);

        //optimization of above Methods
        Task<MemberDto> GetMemberByIdAsync(int id);
        Task<MemberDto> GetMemberByNameAsync(string name);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);


    }
}