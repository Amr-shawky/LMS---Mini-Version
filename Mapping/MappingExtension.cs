using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
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





        public static EnrollmentDTO toEnrollmentDTO(this Enrollment entity)
        {
            return new EnrollmentDTO()
            {
                Id = entity.Id,
                InternId = entity.InternId,
                InternName = entity.Intern.FullName ?? string.Empty,
                TrackId = entity.TrackId,
                TrackName = entity.Track.Name ?? string.Empty,
                EnrollmentDate = entity.EnrollmentDate,
                Status = entity.Status
            };
        }

        public static EnrollmentSummaryDTO ToEnrollmentSummaryDTO(this Enrollment entity)
        {
            return new EnrollmentSummaryDTO()
            {
                Id = entity.Id,
                InternId = entity.InternId,
                InternName = entity.Intern.FullName ?? string.Empty,
                TrackId = entity.TrackId,
                TrackName = entity.Track.Name ?? string.Empty,
                EnrollmentDate = entity.EnrollmentDate,
                Status = entity.Status
            };
        }

        public static InternEnrollmentDto ToEnrollmentInternDto(this Enrollment entity)
        {
            return new InternEnrollmentDto()
            {
                Id = entity.Id,
                InternId = entity.InternId,
                InternName = entity.Intern.FullName ?? string.Empty,
                TrackId = entity.TrackId,
                TrackName = entity.Track.Name ?? string.Empty,
                EnrollmentDate = entity.EnrollmentDate,
                Status = entity.Status
            };
        }





        public static TrackDto toTrackDto(this Track entity)
        {
            return new TrackDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Fees = entity.Fees,
                IsActive = entity.IsActive,
                MaxCapacity = entity.MaxCapacity,

            };
        }


        public static TrackSummaryDTO ToTrackSummaryDto(this Track entity)
        {
            return new TrackSummaryDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Fees = entity.Fees,
                IsActive = entity.IsActive,
                MaxCapacity = entity.MaxCapacity,

            };
        }

    }
}
