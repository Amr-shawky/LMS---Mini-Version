using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetAllIntentsQuery : IRequest<IEnumerable<InternSummaryViewModel>>;
    
}
