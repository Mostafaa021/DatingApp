using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDto>() // to map specefic property (Source , Destination) 
            .ForMember(dest=>dest.PhotoUrl,// specify  Member of Destination
            options=>options.MapFrom(sourceMember=>sourceMember.Photos.FirstOrDefault(s=>s.IsMain).URL)) //Mapping From source Member
            .ForMember(dest=>dest.Age,
            options=>options.MapFrom(src=>src.BirthDate.CalculateAge())); // Here we Mapped Directl from MemberDto => AppUser
            CreateMap<Photo,PhotoDto>();
        }
    }
}