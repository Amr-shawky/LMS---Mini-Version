using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LMS___Mini_Version.CQRS.Intern.Queries.QueryHandler
{
    public class GetByIdInternsQueryHandler : IRequestHandler<GetByIdInternsQuery, ResultResponse<InternDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache cache;

        public GetByIdInternsQueryHandler(IUnitOfWork unitOfWork ,IMemoryCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.cache = cache;
        }
        public async Task<ResultResponse<InternDto>> Handle(GetByIdInternsQuery request, CancellationToken cancellationToken)
        {
            var intern = await unitOfWork.Interns
                  .GetTable()
                  .Include(i => i.Track)
                  .FirstOrDefaultAsync(i => i.Id == request.Id);
            if (intern == null)
            {
                return ResultResponse<InternDto>.Faild("The Id is Not Correct");
            }

            var Dto = intern.ToDto();
            cache.Set($"intern", Dto);
            return ResultResponse<InternDto>.Success(Dto);


        }
    }
}
