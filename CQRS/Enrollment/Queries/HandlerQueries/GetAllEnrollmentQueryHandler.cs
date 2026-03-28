using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.HandlerQueries
{
    public class GetAllEnrollmentQueryHandler : IRequestHandler<GetAllEnrollmentQuery, RequestResult<IEnumerable<EnrollmentDto>>>
    {
        private readonly IUnitOfWork _uow;
        public async Task<RequestResult<IEnumerable<EnrollmentDto>>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var enrollmentsDto= await _uow.Enrollments.GetTable().Select(enrollment => enrollment.ToDto()).ToListAsync();
            return (enrollmentsDto==null)
                ?RequestResult<IEnumerable<EnrollmentDto>>.Failure(ErrorCode.NotFound) 
                :RequestResult<IEnumerable<EnrollmentDto>>.Success(enrollmentsDto);
        }
    }
}
