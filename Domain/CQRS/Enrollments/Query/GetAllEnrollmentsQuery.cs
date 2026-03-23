using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Enrollments.Query
{
    public record GetAllEnrollmentsQuery: IRequest<IEnumerable<EnrollmentDTO>>;
   
}
