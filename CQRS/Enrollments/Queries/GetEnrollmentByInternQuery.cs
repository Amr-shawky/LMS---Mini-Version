using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries
{
    public record GetEnrollmentByInternQuery(int InternId) : IRequest<IEnumerable<EnrollmentViewModel>>;
}
