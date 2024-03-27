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
               options=>options.MapFrom(src=>src.BirthDate.CalculateAge())).ReverseMap(); // Here we Mapped Direct from Appuser => MemberDto and Reverse
            CreateMap<Photo,PhotoDto>().ReverseMap();
            CreateMap<MemberUpdateDto,AppUser>().ReverseMap();
            CreateMap<RegisterDTO,AppUser>();
            CreateMap<Message, MessageDto>() // to map specefic property (Source , Destination) 
                .ForMember(dest => dest.SenderPhotoUrl,
                   options => options.MapFrom(soruceMember => soruceMember.Sender.Photos.FirstOrDefault(x => x.IsMain).URL))
                .ForMember(dest => dest.RecipientPhotoUrl,
                   options => options.MapFrom(sourceMember => sourceMember.Recipient.Photos.FirstOrDefault(x => x.IsMain).URL))
                .ReverseMap(); 


        }
    }
}