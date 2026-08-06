using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto>
    {
        private readonly IGeneralRepository<Intern> _intern;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> intern)
        {
            _intern = intern;
        }

        public async Task<InternDto> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {

     
            var intern = await _intern.GetByIdAsync(request.id);                
            return intern?.ToDto();
        }




    }
}
