using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.DTOS;
using LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.Handler
{
    public class GetTrackByAvailabilityHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTrackByAvailabilityQuery, RequestResponse<GetTrackByAvailabilityDTO>>
    {
        public async Task<RequestResponse<GetTrackByAvailabilityDTO>> Handle(GetTrackByAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var TrackId = request.TrackId;
            var TrackRepo = unitOfWork.GetRepository<Track>();

            var track = await TrackRepo.GetByIdAsync(TrackId);
            if (track == null)
                return RequestResponse<GetTrackByAvailabilityDTO>.Fail(StatusCodes.Status400BadRequest,
        new Dictionary<string, string[]> { ["trackNotFound"] = [$"Track with id {TrackId} is not found"] });

            var reservedSeats = unitOfWork.GetRepository<Enrollment>()
             .GetAll()
             .Count(e => e.TrackId == track.Id &&
                        (e.Status == EnrollmentStatus.Pending ||
                         e.Status == EnrollmentStatus.Active));

            var maxCapacity = track.MaxCapacity;
            var availableSeats = maxCapacity - reservedSeats;
            if (availableSeats < 0)
                return RequestResponse<GetTrackByAvailabilityDTO>.Fail(StatusCodes.Status400BadRequest,
        new Dictionary<string, string[]> { ["AVailableSeats"] = ["Available Seats cant be negative values"] });

            var dto = new GetTrackByAvailabilityDTO(track.Name, track.IsActive, reservedSeats, maxCapacity, availableSeats);

            return RequestResponse<GetTrackByAvailabilityDTO>.OK(dto);

        }
    }
}
