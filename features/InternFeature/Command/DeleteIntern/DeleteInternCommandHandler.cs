using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Command.DeleteIntern;

public class DeleteInternCommandHandler(
    IGeneralRepository<Intern> internRepository,
    IGeneralRepository<Enrollment> enrollmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteInternCommand, RequestResponse>
{
    private const string HasEnrollmentsMessage = "Intern has enrollment history and cannot be deleted";

    public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {
        // intern must exist
        var intern = await internRepository.GetByIdAsync(request.InternId);
        if (intern is null)
            return Fail(404, "InternId", "Intern not found");

        // any enrollment (Pending, Active, Completed or Cancelled) blocks deletion
        var hasEnrollments = await enrollmentRepository.GetAll()
            .AnyAsync(e => e.InternId == request.InternId, cancellationToken);
        if (hasEnrollments)
            return Fail(409, "Intern", HasEnrollmentsMessage);

        // delete the intern only
        internRepository.Delete(intern);

        await unitOfWork.SaveChangesAsync();
     

        return RequestResponse.OK();
    }

    private static RequestResponse Fail(int code, string key, string message)
        => RequestResponse.Fail(code, new Dictionary<string, string[]> { { key, [message] } });
}