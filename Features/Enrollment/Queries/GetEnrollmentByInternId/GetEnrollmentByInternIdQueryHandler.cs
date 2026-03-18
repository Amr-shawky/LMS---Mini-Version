using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetEnrollmentByInternId
{
    public class GetEnrollmentByInternIdQueryHandler : IRequestHandler<GetEnrollmentByInternIdQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentByInternIdQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Track)
                .Include(e => e.Intern)
                .Where(e => e.InternId ==request.internId)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => e.ToDto());
        }
    }
}
