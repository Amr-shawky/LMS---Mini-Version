using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTO;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Handler
{
    public class CheckTrackAvaiabilityQueryHandler : IRequestHandler<CheckTrackAvaiabilityQuery, RequestResponse<CheckTrackAvailabilityDTO>>
    {
        private readonly IGeneralRepository<Track> trackRepo;
        

        public CheckTrackAvaiabilityQueryHandler(IGeneralRepository<Track> trackRepo)
        {
            this.trackRepo = trackRepo;
            
        }
        public async Task<RequestResponse<CheckTrackAvailabilityDTO>> Handle(CheckTrackAvaiabilityQuery request, CancellationToken cancellationToken)
        {
            var track = await trackRepo.GetAll().Where(x => x.Id == request.trackId).Select(x => new
            {
                IsActive = x.IsActive,
                maxCapacity = x.MaxCapacity,
                trackName = x.Name,
                reservedSeats = x.Enrollments.Count(e => e.Status == Domain.Enums.EnrollmentStatus.Pending || e.Status==Domain.Enums.EnrollmentStatus.Active),
                
            }).FirstOrDefaultAsync();
            if (track == null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Track" , new[]{"Track Doesnot Esists"} }
                };
                return RequestResponse<CheckTrackAvailabilityDTO>.Fail(404, errors);

            }

           
            int availableSeats = track.maxCapacity - track.reservedSeats;
            if(availableSeats < 0)
            {
                availableSeats = 0;
            }
          
            var dto = new CheckTrackAvailabilityDTO()
            {
                AvailableSeats = availableSeats,
                MaxCapacity = track.maxCapacity,
                IsActive = track.IsActive,
                ReservedSeats = track.reservedSeats,
                TrackName = track.trackName,

            };
            return RequestResponse<CheckTrackAvailabilityDTO>.OK(dto , 200);

        }
    }
}
