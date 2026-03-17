using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetEnrollmentById;

public class GetEnrollmentByIdQueryHandler  :IRequestHandler<GetEnrollmentById.GetEnrollmentByIdQuery,EnrollmentDto>
{
    private readonly IUnitOfWork _uow;
    public GetEnrollmentByIdQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<EnrollmentDto> Handle(GetEnrollmentById.GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _uow.Enrollments.GetByIdAsync(request.EnrollmentId).ConfigureAwait(false);
        if (enrollment == null)
            throw new InvalidOperationException($"enrollment with Id {request.EnrollmentId} Doesn't exist");
        return enrollment.ToDto();
    }
}