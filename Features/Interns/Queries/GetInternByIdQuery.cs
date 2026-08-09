using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetInternByIdQuery(int Id) : IRequest<InternDto>;

    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto>
    {
        private readonly IGeneralRepository<Intern> _internrepository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internrepository)
        {
            _internrepository = internrepository;
        }

        public async Task<InternDto> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _internrepository.GetByIdAsync(request.Id);
            return intern.ToDto();
        }
    }
}
