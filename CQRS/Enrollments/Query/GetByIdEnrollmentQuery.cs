using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Query
{
    public record GetByIdEnrollmentQuery(int Id):IRequest<RequestResult<EnrollmentSummaryDTO?>>;

   
}
