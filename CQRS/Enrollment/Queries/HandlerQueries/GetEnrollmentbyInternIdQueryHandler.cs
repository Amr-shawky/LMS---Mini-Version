using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.HandlerQueries
{
    public class GetEnrollmentbyInternIdQueryHandler : IRequestHandler<GetEnrollmentbyInternIdQuery, RequestResult<IEnumerable<EnrollmentDto>>>
    {
        private readonly IUnitOfWork _uow;

        public async Task<RequestResult<IEnumerable<EnrollmentDto>>> Handle(GetEnrollmentbyInternIdQuery request, CancellationToken cancellationToken)
        {
            var enrollmentsDto = await _uow.Enrollments.GetTable().Where(en => en.InternId == request.internId).Select(enrollment => enrollment.ToDto()).FirstOrDefaultAsync();
            return (enrollmentsDto == null)
                ? RequestResult<EnrollmentDto>.Failure(ErrorCode.NotFound)
                : RequestResult<EnrollmentDto>.Success(enrollmentsDto);
        }
    }
}
