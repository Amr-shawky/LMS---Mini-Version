using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace LMS___Mini_Version.CQRS.Intern.Queries.HandlerQueries
{
    public class GetInternbyIdQueryHandler : IRequestHandler<GetInternbyIdQuery, RequestResult<InternDto>>
    {
        private readonly IUnitOfWork _uow;
        public async Task<RequestResult<InternDto>> Handle(GetInternbyIdQuery request, CancellationToken cancellationToken)
        {
            var internDto = await _uow.Interns.GetTable().Where(i => i.Id == request.internId).Select(intern=>intern.ToDto()).FirstOrDefaultAsync();
            return (internDto == null)
             ? RequestResult<InternDto>.Failure(ErrorCode.NotFound)
             : RequestResult<InternDto>.Success(internDto);
        }
    }
}
