using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class updateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> repository)
    : IRequestHandler<updateEnrollmentStatusCommand, RequestResponse<bool>>
{
    public async Task<RequestResponse<bool>> Handle(updateEnrollmentStatusCommand request, CancellationToken ct)
    {
        if (!await repository.GetAll().AnyAsync(e => e.Id == request.enrollmentId, ct))
            return RequestResponse<bool>.Fail("Enrollment not found", 404);
        repository.SaveInclude(new Enrollment { Id = request.enrollmentId, Status = request.Status },
            nameof(Enrollment.Status));
        await repository.SaveChangesAsync();
        return RequestResponse<bool>.Ok(true);
    }
}
