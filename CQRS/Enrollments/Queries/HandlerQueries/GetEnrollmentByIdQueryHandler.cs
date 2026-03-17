using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.HandlerQueries
{
    public class GetEnrollmentByIdQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentViewModel>
    {
        public async Task<EnrollmentViewModel> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments.GetTable().Include(e => e.Intern).Include(e => e.Track).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (enrollment == null)
                throw new Exception($"Enrollment with Id {request.Id} not found.");
            return new EnrollmentViewModel
            {
                Id = enrollment.Id,
                InternName = enrollment.Intern.FullName,
                TrackName = enrollment.Track.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status.ToString()
            };
        }
    }
}
