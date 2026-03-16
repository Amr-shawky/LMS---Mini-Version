using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Queries
{
    public record GetInternByIdQuery(int Id) : IRequest<InternDetailViewModel>;

    
}
