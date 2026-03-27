using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Queries.HandlerQueries
{
    public class GetAllInternQueryHandler : IRequestHandler<GetAllInternQuery, RequestResult<IEnumerable<InternDto>>>
    {
        private readonly IUnitOfWork _uow;

        public async Task<RequestResult<IEnumerable<InternDto>>> Handle(GetAllInternQuery request, CancellationToken cancellationToken)
        {
            var internDto = await _uow.Interns.GetTable().Select(intern => intern.ToDto()).ToListAsync();
            return (internDto == null)
                ? RequestResult<IEnumerable<InternDto>>.Failure(ErrorCode.NotFound)
                : RequestResult<IEnumerable<InternDto>>.Success(internDto);
        }
    }
}
