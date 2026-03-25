using LMS___Mini_Version.CQRS.Enrollments.Query;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class GetByIdEnrollmentQueryHandler : IRequestHandler<GetByIdEnrollmentQuery, RequestResult<EnrollmentSummaryDTO?>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        public GetByIdEnrollmentQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository=repository;
        }
        public async Task<RequestResult<EnrollmentSummaryDTO?>> Handle(GetByIdEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var Enroll = await _repository.GetTable()
                        .Include(e=>e.Track)
                        .Include(e=>e.Intern)
                        .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
                        .ConfigureAwait(false);
            if (Enroll == null)
            {
                return RequestResult<EnrollmentSummaryDTO?>.Failure(ErrorCode.EnrollMentNotExist);
            }

            return RequestResult<EnrollmentSummaryDTO?>.Success(Enroll.ToEnrollmentSummaryDTO());

        }
    }
}
