using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Dtos;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Handler
{
    public class GetTrackAvailabilityQueryHandler : IRequestHandler<GetTrackAvailabilityQuery, RequestResponse<TrackAvailabilityResponseDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IGeneralRepository<Track> _trackRepository;

        public GetTrackAvailabilityQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository , IGeneralRepository<Track> trackRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _trackRepository = trackRepository;
        }
        public async Task<RequestResponse<TrackAvailabilityResponseDto>> Handle(GetTrackAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetByIdAsync(request.TrackId);
            if(track is null)
            {
                return RequestResponse<TrackAvailabilityResponseDto>.Fail(
                404,
                new Dictionary<string, string[]>
                {
                    { "TrackId", new[] { "Track not found." } }
                });
            }

            var reservedSeats = await _enrollmentRepository
                                .GetAll()
                                .CountAsync(
                                x => x.TrackId == request.TrackId &&
                                (x.Status == EnrollmentStatus.Pending || x.Status == EnrollmentStatus.Active),
                                cancellationToken);
            var availableSeats = Math.Max(0, track.MaxCapacity - reservedSeats);
            var response = new TrackAvailabilityResponseDto(
                                track.Name,
                                track.IsActive,
                                track.MaxCapacity,
                                reservedSeats,
                                availableSeats
                            );

            return RequestResponse<TrackAvailabilityResponseDto>.OK(response);
        }
    }
}
