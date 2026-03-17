using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Queries
{
    public record GetAllInternsQuery : IQuery<IEnumerable<InternDto>> { }

    public class GetAllInternsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        public async Task<IEnumerable<InternDto>> IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>.Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Domain.Entities.Intern>();


            var interns = repo.GetAll()
                              .Select(i => i.ToDto());

           return interns;
        }
    }
}
