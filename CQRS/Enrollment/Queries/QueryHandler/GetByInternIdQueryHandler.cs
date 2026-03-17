using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.QueryHandler
{
    public class GetByInternIdQueryHandler : IRequestHandler<GetByInternIdQuery, ResultResponse<IEnumerable<EnrollmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache cache;

        public GetByInternIdQueryHandler(IUnitOfWork unitOfWork,IMemoryCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.cache = cache;
        }
        public async Task<ResultResponse<IEnumerable<EnrollmentDto>>> Handle(GetByInternIdQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await unitOfWork.Enrollments
               .GetTable()
               .Include(e => e.Track)
               .Include(e => e.Intern)
               .Where(e => e.InternId == request.internId)
               .ToListAsync();

            if (!enrollments.Any())
            {
                return ResultResponse<IEnumerable<EnrollmentDto>>.Faild("Not Found Any Interns");
            }

            var enrollmentsDto = enrollments.Select(e => e.ToDto()).Skip((request.page - 1) * 5).Take(5);
            cache.Set($"AllInternDto", enrollmentsDto);
            return ResultResponse<IEnumerable<EnrollmentDto>>.Success(enrollmentsDto);

        }
    }
}
