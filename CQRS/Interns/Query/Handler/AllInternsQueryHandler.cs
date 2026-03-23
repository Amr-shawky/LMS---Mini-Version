using LMS___Mini_Version.CQRS.Interns.Query;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Interns.Query.Handler
{
    public class AllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDTO>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public AllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _repository = internRepository;
        }
        public async Task<IEnumerable<InternDTO>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var interns = await _repository
                            .GetTable()
                            .Include(i => i.Track).ToListAsync();
      
                var internDtos = interns.Select(i => i.ToDo());
                return internDtos;
            }
            catch (Exception ex) 
            {

                throw new NotImplementedException(ex.Message);
            }
            

            
        }
    }
}
