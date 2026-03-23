using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Query.Handler
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternSummaryDTO>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        async Task<InternSummaryDTO> IRequestHandler<GetInternByIdQuery, InternSummaryDTO>.Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            //var intern=await _repository.GetById(request.Id);

            var intern = await _repository
               .GetTable()
               .Include(i => i.Track) 
               .FirstOrDefaultAsync(i => i.Id == request.Id);


            if (intern == null)
            {

                throw new NotImplementedException();
            }
            var internDto= intern.ToInternSummaryDTO();
            return internDto;

        }
    }
}
