using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using LMS___Mini_Version.ViewModels.InternViewModels;

namespace LMS___Mini_Version.Mapping
{
    public static class MappingExtension
    {
        public static InternDTO ToDo(this Intern entity)
        {
            return new InternDTO()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                BirthYear = entity.BirthYear,
                Status = entity.Status,
                TrackId = entity.TrackId,
                TrackName = entity.Track?.Name ?? string.Empty

            };
        }

        public static internVM ToDo(this InternDTO entity)
        {
            return new internVM()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
               
                Status = entity.Status,
              
                TrackName = entity.TrackName
            };
        }



        public static InternSummaryDTO ToInternSummaryDTO(this Intern entity) => new InternSummaryDTO()
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            BirthYear= entity.BirthYear,
            Status = entity.Status,
            TrackId = entity.TrackId,
            TrackName= entity.Track?.Name ?? string.Empty   
        };

    }
}
