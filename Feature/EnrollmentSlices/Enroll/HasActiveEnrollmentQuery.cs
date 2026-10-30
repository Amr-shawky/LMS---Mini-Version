using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries;

public record HasActiveEnrollmentQuery(int InternId, int TrackId) : IRequest<RequestResponse<bool>>;

public sealed class HasActiveEnrollmentQueryHandler(IGeneralRepository<Enrollment> repository)
    : IRequestHandler<HasActiveEnrollmentQuery, RequestResponse<bool>>
{
    public async Task<RequestResponse<bool>> Handle(HasActiveEnrollmentQuery req, CancellationToken ct)
    {
        var exists = await repository.GetAll().AnyAsync(e => e.InternId == req.InternId &&
            e.TrackId == req.TrackId && e.Status != EnrollmentStatus.Cancelled, ct);
        return RequestResponse<bool>.Ok(exists);
    }
}
