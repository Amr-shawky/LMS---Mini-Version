using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability
{
    public class TrackAvailabilityHandler : IRequestHandler<TrackAvailabilityQuery, RequestResponse<TrackAvailabilityDto>>
    {
        readonly private IGeneralRepository<Track> _trackRepository;
        public TrackAvailabilityHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }
        public async Task<RequestResponse<TrackAvailabilityDto>> Handle(TrackAvailabilityQuery request, CancellationToken cancellationToken)
        {
            TrackAvailabilityDto trackAvailabilityDto = _trackRepository.GetAll().Where(t => t.Id == request.trackId).Select(t => new TrackAvailabilityDto
            {
                TrackName = t.Name,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity,
                ReservedSeats = t.Enrollments.Count(e => e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending ),
                AvailableSeats = t.Enrollments.Count(e => e.Status == EnrollmentStatus.Cancelled || e.Status == EnrollmentStatus.Completed)
            }).FirstOrDefault();
            if (trackAvailabilityDto is null)
                return RequestResponse<TrackAvailabilityDto>.Fail(404, ErrorCode.TrackNotFound);
            return RequestResponse<TrackAvailabilityDto>.OK(trackAvailabilityDto);



        }
    }
}

