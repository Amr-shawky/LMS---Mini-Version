using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.ViewModels.Intern;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDetailViewModel>
    {
        private readonly IGeneralRepository<Intern> _internRepo; 

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepo)
        {
            _internRepo = internRepo;
        }
        public async Task<InternDetailViewModel> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _internRepo.GetByIdAsync(request.id);
            return intern?.ToDto().ToDetailViewModel();
        }
    }
}
