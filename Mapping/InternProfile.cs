using AutoMapper;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;

namespace LMS___Mini_Version.Mapping
{
    public class InternProfile : Profile
    {

        public InternProfile() 
        {

            CreateMap<Intern, InternDTO>()
                .ForMember(dest=>dest.TrackName,
                            opt=>opt.MapFrom(src=>src.Track.Name))
                .ForMember(dest=>dest.TrackId,
                            opt=>opt.MapFrom(src=>src.TrackId));


        }
    }
}
