using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollmentsByInternId;

public class GetEnrollmentsByInternIdHandler : IRequestHandler<GetEnrollmentsByInternId,IEnumerable<EnrollmentDto>>
{
    private readonly IUnitOfWork _uow;
    public GetEnrollmentsByInternIdHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternId request, CancellationToken cancellationToken)
    {
        var intern = await _uow.Interns.GetByIdAsync(request.InternId).ConfigureAwait(false);
        if(intern == null) throw new InvalidOperationException($"Intern with Id {request.InternId} Doesn't exist");
        var enrollments = await _uow.Enrollments.GetAllByInternIdAsync(request.InternId).ConfigureAwait(false);
        return enrollments.Select(e => e.ToDto());
    }
}