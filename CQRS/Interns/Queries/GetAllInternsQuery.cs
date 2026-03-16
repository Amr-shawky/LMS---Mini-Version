using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Queries
{
    public record GetAllInternsQuery : IRequest<IEnumerable<InternSummaryViewModel>>;
    
}
