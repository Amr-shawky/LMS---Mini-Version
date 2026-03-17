using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetActiveEnrollmentByInternId;

public class GetActiveEnrollmentByInternIdQueryHandler : IRequestHandler<GetActiveEnrollmentByInternIdQuery,EnrollmentDto>
{
    private readonly IUnitOfWork _uow;
    public GetActiveEnrollmentByInternIdQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<EnrollmentDto> Handle(GetActiveEnrollmentByInternIdQuery request, CancellationToken cancellationToken)
    {
        var intern = await _uow.Interns.GetByIdAsync(request.InternId).ConfigureAwait(false);
        if(intern == null) throw new InvalidOperationException($"Intern with Id {request.InternId} Doesn't exist");
        var enrollment = await _uow.Enrollments.GetActiveEnrollmentByInternIdAsync(request.InternId).ConfigureAwait(false);
        if (enrollment == null)
            throw new InvalidOperationException($"This Intern with Id {request.InternId} doesn't have an enrollment ");
        return enrollment.ToDto();
    }
}