using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries.HandlerQueries
{
    public class IsEnrollmentExistQueryHandler : IRequestHandler<IsEnrollmentExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public async Task<RequestResult<bool>> Handle(IsEnrollmentExistQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _uow.Enrollments.GetTable().AnyAsync(enrollment => enrollment.TrackId == request.trackId&& enrollment.InternId== request.internId);
            return RequestResult<bool>.Success(enrollment);
        }
    }
}
