using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public class GetEnrollmentsByInternQuery:IRequest<IEnumerable<EnrollmentDto>>
    {
        public int InternId { get; set; }
    }
}
