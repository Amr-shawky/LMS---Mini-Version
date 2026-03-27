using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.HandlerQueries
{
    public class GetEnrollmentbyIdQueryHandler : IRequestHandler<GetEnrollmentbyIdQuery, RequestResult<EnrollmentDto>>
    {
        private readonly IUnitOfWork _uow;

        public async Task<RequestResult<EnrollmentDto>> Handle(GetEnrollmentbyIdQuery request, CancellationToken cancellationToken)
        {
            var enrollmentsDto = await _uow.Enrollments.GetTable().Where(en=>en.TrackId==request.id).Select(enrollment => enrollment.ToDto()).FirstOrDefaultAsync();
            return (enrollmentsDto==null)
                ? RequestResult<EnrollmentDto>.Failure(ErrorCode.NotFound)
                : RequestResult<EnrollmentDto>.Success(enrollmentsDto);
        }
    }
}
