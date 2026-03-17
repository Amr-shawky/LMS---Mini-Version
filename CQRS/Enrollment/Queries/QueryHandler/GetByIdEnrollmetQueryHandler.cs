using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.QueryHandler
{
    public class GetByIdEnrollmetQueryHandler : IRequestHandler<GetByIdEnrollmentQuery, ResultResponse<EnrollmentDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache cache;

        public GetByIdEnrollmetQueryHandler(IUnitOfWork unitOfWork,IMemoryCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.cache = cache;
        }

        public async Task<ResultResponse<EnrollmentDto>> Handle(GetByIdEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            if (enrollment == null)
            {
                return ResultResponse<EnrollmentDto>.Faild("The Id is Not Correct");
            }

            var Dto = enrollment.ToDto();
            cache.Set($"intern", Dto);
            return ResultResponse<EnrollmentDto>.Success(Dto);
        }
    }
}
