using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetAllEnrollment
{
    public class GetAllEnrollmentQueryHandler : IRequestHandler<GetAllEnrollmentQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllEnrollmentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<EnrollmentDto>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => e.ToDto());
        }
    }
}
