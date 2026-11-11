using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.handler
{
    public class GetTrackAvailabilityQueryHandler : IRequestHandler<GetTrackAvailabilityQuery, RequestResponse<GetTrackAvailabilityDto>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public GetTrackAvailabilityQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<GetTrackAvailabilityDto>> Handle(GetTrackAvailabilityQuery request, CancellationToken cancellationToken)
        {
            if (request.TrackId <= 0)
            {
                return RequestResponse<GetTrackAvailabilityDto>.Fail(400, new Dictionary<string, string[]> { { "TrackId", new[] { "TrackId must be greater than 0." } } });
            }
            var trackData = await _repository.GetAll()
                .Where(t => t.Id == request.TrackId)
                .Select(t => new
                {
                    t.Name,t.IsActive,t.MaxCapacity,
                    ReservedSeats = t.Enrollments.Count(e =>e.Status == EnrollmentStatus.Active ||e.Status == EnrollmentStatus.Pending)
                }).FirstOrDefaultAsync(cancellationToken);

            if (trackData == null)
            {
                return RequestResponse<GetTrackAvailabilityDto>.Fail(404, new Dictionary<string, string[]> { { "Track", new[] { "Track not found." } } });
            }
            int availableSeats = trackData.MaxCapacity - trackData.ReservedSeats;
            if(availableSeats < 0)
            {
                availableSeats = 0;
            }
            var result = new GetTrackAvailabilityDto
            {
                TrackName = trackData.Name,
                IsActive = trackData.IsActive,
                MaxCapacity = trackData.MaxCapacity, 
                ReservedSeats = trackData.ReservedSeats,
                AvailableSeats = availableSeats
            };
            return RequestResponse<GetTrackAvailabilityDto>.OK(result,200);
        }
    }
}
