using API.DTOs;
using API.Entities;
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
        public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
        {
            // No Need for this function if using AutoMapper
             return   await _context.Users
             .Include(p=>p.Photos)
             .ToListAsync();
        }


        public  async Task<AppUser> GetUserByIdAsync(int id)
        {// No Need for this function if using AutoMapper
            return await _context.Users
            .Include(p=>p.Photos)
            .FirstOrDefaultAsync(s=>s.Id == id);
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

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
             // AutoMapper is Eager Loading so no need to use Include
            return await _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }
    }
}