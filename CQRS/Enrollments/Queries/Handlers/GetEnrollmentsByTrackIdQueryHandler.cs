using LMS___Mini_Version.CQRS.Enrollments.Queries.Requests;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Handlers
{
    public class GetEnrollmentsByTrackIdQueryHandler : IRequestHandler<GetEnrollmentsByTrackIdQuery, IEnumerable<EnrollmentDto>>
    {

        private readonly IUnitOfWork _unitOf;
        public GetEnrollmentsByTrackIdQueryHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByTrackIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _unitOf.Tracks.GetByIdAsync(request.TrackId);
            if (track == null)
                throw new InvalidOperationException($"Track with Id {request.TrackId} doesn't exist");

            var enrollments = await _unitOf.Enrollments.GetByTrackIdWithDetailsAsync(request.TrackId);
            return enrollments.Select(e => e.ToDto());
        }

    }
}
