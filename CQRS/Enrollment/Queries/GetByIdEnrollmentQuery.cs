using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record GetByIdEnrollmentQuery(int Id,CancellationToken CancellationToken)
        :IRequest<ResultResponse<EnrollmentDto>>;
     
}
