using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record GetByInternIdQuery(int internId,CancellationToken CancellationToken,int page=1)
        :IRequest<ResultResponse<IEnumerable<EnrollmentDto>>>;
   
}
