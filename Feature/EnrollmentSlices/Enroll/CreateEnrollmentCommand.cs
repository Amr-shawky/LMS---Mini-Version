using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands;

public record CreateEnrollmentCommand(int InternId, int TrackId) : IRequest<RequestResponse<int>>;

public class CreateEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository)
    : IRequestHandler<CreateEnrollmentCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(CreateEnrollmentCommand request, CancellationToken ct)
    {
        var entity = new Enrollment {
            InternId = request.InternId, TrackId = request.TrackId,
            Status = EnrollmentStatus.Active, EnrollmentDate = DateTime.UtcNow
        };
        repository.Add(entity);
        await repository.SaveChangesAsync();
        return RequestResponse<int>.Created(entity.Id);
    }
}
