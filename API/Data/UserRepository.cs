using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context , IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
         public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            // get all user asQueryable
            var query = _context.Users.AsQueryable();
            /// Adding some Filters 
            // get users that not matching the same username(other users) && Gender == Gender
            // here user.Gender == userParams.Gender not the oposite to be handled in controller
            // but user.userName != userParams.userName as in all cases you can`t matching yourself 
           query = query.Where(x=>x.UserName != userParams.CurrentUserName && x.Gender == userParams.Gender);
           var MinBirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-userParams.MaxAge-1)) ;  // to get minimum date for app
           var MaxBirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-userParams.MinAge)) ;  // to get Max date  for app
           query= query.Where(x=>x.BirthDate >= MinBirthDate && x.BirthDate <= MaxBirthDate);
           query = userParams.OrderBy switch 
           {
            // if order by created 
               "created" => query.OrderByDescending(u=>u.Created),
               // default case for switch statment which declared in userParams = "lastActive"
                _ => query.OrderByDescending(u=>u.LastActive)
           };
            // No need to make EF core track PageList Class as it`s not the actual user
           var items = query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
            return await PagedList<MemberDto>.CreateAsync(items,
            userParams.PageSize ,userParams.PageNumber);
        }
         public  async Task<AppUser> GetUserByIdAsync(int id)
        {// No Need for this function if using AutoMapper
            return await _context.Users.FindAsync(id);
        }
        public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
        {
            // No Need for this function if using AutoMapper
             return   await _context.Users
             .ToListAsync();
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {// No Need for this function if using AutoMapper
         
              return await _context.Users
              .Include(p=>p.Photos)
              .SingleOrDefaultAsync(s=>s.UserName == username);
        }

        public  async Task<bool> SaveAllAsync()
        {
             // if changes happened in database then will return more than 0 row affect with True
             // nothing happened then will return 0 ( no changes) with fasle 
            return await _context.SaveChangesAsync() > 0;
        }                                       

        public void Update(AppUser user)
        {
           _context.Entry(user).State = EntityState.Modified; // tell the EF tracker that entity has been updated 
        }
        // Optimization
        public  async Task<MemberDto> GetMemberByIdAsync(int id)
        {
             // AutoMapper is Eager Loading so no need to use Include
            return await _context.Users
            .Where(p=>p.Id == id)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberByNameAsync(string name)
        {
            // AutoMapper is Eager Loading so no need to use Include
            return await _context.Users
            .Where(p=>p.UserName == name)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

       


    }
}