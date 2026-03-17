using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollments;

public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollments.GetAllEnrollmentsQuery,IEnumerable<EnrollmentDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllEnrollmentsQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IEnumerable<EnrollmentDto>> Handle(GetAllEnrollments.GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _uow.Enrollments.GetAllAsync().ConfigureAwait(false);
        return enrollments.Select(e => e.ToDto());
    }
}