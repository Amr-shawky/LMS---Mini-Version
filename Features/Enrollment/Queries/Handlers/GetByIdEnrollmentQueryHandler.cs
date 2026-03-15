using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.Queries.Handlers
{
    public class GetByIdEnrollmentQueryHandler : IRequestHandler<GetByIdEnrollmentQuery, EnrollmentViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdEnrollmentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<EnrollmentViewModel> Handle(GetByIdEnrollmentQuery request, CancellationToken cancellationToken)
        {

            var enrollment = await _unitOfWork.Enrollments
                   .GetTable()
                   .Include(e => e.Intern)
                   .Include(e => e.Track)
                   .FirstOrDefaultAsync(e => e.Id == request.id, cancellationToken);

            if (enrollment == null)
                throw new NotFoundException("Enrollment",request.id);

            var enrollmentViewModel = new EnrollmentViewModel
            {
                Id = enrollment.Id,
                InternName = enrollment.Intern?.FullName ?? "N/A",
                TrackName = enrollment.Track?.Name ?? "N/A",
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment?.Status != null ? enrollment.Status.ToString() : "Unknown"
            };

            return enrollmentViewModel;



        }
    }
}
