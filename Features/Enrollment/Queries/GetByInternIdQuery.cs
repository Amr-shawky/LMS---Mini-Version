using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Queries
{
    public record GetByInternIdQuery(int InternId) : IRequest<IEnumerable<EnrollmentViewModel>>;
    
}
