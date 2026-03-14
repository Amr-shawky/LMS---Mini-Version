using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetEnrollmentsByTrackId;

public class GetEnrollmentsByTrackIdQueryHandler:IRequestHandler<GetEnrollmentsByTrackIdQuery,IEnumerable<EnrollmentDto>>
{
    private readonly IUnitOfWork _uow;
    public GetEnrollmentsByTrackIdQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByTrackIdQuery request, CancellationToken cancellationToken)
    {
        var track = await _uow.Tracks.GetByIdAsync(request.TrackId);
        if(track == null) throw new InvalidOperationException($"Track with Id {request.TrackId} doesn't exist");
        var enrollments = await _uow.Enrollments.GetByTrackIdWithDetailsAsync(request.TrackId).ConfigureAwait(false);
        return enrollments.Select(e => e.ToDto());
    }
}