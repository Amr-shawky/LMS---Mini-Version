using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetEnrollmentById
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEnrollmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<EnrollmentDto?> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments
              .GetTable()
              .Include(e => e.Intern)
              .Include(e => e.Track)
              .FirstOrDefaultAsync(e => e.Id == request.id)
              .ConfigureAwait(false);

            return enrollment?.ToDto();
        }
    }
}
