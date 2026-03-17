using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LMS___Mini_Version.CQRS.Intern.Queries.CommandHandler
{
    public class GetAllInternQueryHandler : IRequestHandler<GetAllInternsQuery, ResultResponse<IEnumerable<InternDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache cache;

        public GetAllInternQueryHandler(IUnitOfWork unitOfWork,IMemoryCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.cache = cache;
        }
        public async Task<ResultResponse<IEnumerable<InternDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await unitOfWork.Interns
                .GetTable()
                .Include(i => i.Track)
                .ToListAsync();

            if (!interns.Any())
            {
                return ResultResponse<IEnumerable<InternDto>>.Faild("Not Found Any Interns");
            }

            var internDto=interns.Select(i => i.ToDto()).Skip((request.page-1)*5).Take(5);
            cache.Set($"AllInternDto", internDto);
            return ResultResponse<IEnumerable<InternDto>>.Success(internDto);






        }
    }
}
