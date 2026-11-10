using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTO;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Handler
{
    public class CheckTrackAvaiabilityQueryHandler : IRequestHandler<CheckTrackAvaiabilityQuery, RequestResponse<CheckTrackAvailabilityDTO>>
    {
        private readonly IGeneralRepository<Track> trackRepo;
        private readonly IMediator mediator;

        public CheckTrackAvaiabilityQueryHandler(IGeneralRepository<Track> trackRepo, IMediator mediator)
        {
            this.trackRepo = trackRepo;
            this.mediator = mediator;
        }
        public async  Task<RequestResponse<CheckTrackAvailabilityDTO>> Handle(CheckTrackAvaiabilityQuery request, CancellationToken cancellationToken)
        {
            var track = await trackRepo.GetByIdAsync(request.trackId);
            if (track == null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Track" , new[]{"Track Doesnot Esists"} }
                };
                return RequestResponse<CheckTrackAvailabilityDTO>.Fail(404, errors);

            }



            var entrollemntdResponse = await mediator.Send(new GetEnrollementByTrackIdQuery(request.trackId));
            if(!entrollemntdResponse.Success)
            {
                return RequestResponse<CheckTrackAvailabilityDTO>.Fail(entrollemntdResponse.StatusCode, entrollemntdResponse.Errors);
            }
            int reserveSeats = 0;
            if (entrollemntdResponse.Data !=null)
            {
                var statuses = entrollemntdResponse.Data.Select(x => x.enrollementStatus).ToList();
              
                foreach(var status in statuses)
                {
                    if(status is Domain.Enums.EnrollmentStatus.Pending || status is Domain.Enums.EnrollmentStatus.Active)
                    {
                        reserveSeats++;
                    }
                }

            }
            int AvailableSeats = track.MaxCapacity - reserveSeats;
            if (AvailableSeats< 0)
            {
                AvailableSeats = 0;
            }
            var dto = new CheckTrackAvailabilityDTO()
            {
                AvailableSeats = AvailableSeats,
                MaxCapacity = track.MaxCapacity,
                IsActive = track.IsActive,
                ReservedSeats = reserveSeats,
                TrackName = track.Name,

            };
            return RequestResponse<CheckTrackAvailabilityDTO>.OK(dto , 200);

        }
    }
}
