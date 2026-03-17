using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.HandlerQueries
{
    public class GetAllEnrollmentQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllEnrollmentQuery, IEnumerable<EnrollmentViewModel>>
    {
        public async Task<IEnumerable<EnrollmentViewModel>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _unitOfWork.Enrollments.GetTable().Include(e => e.Intern).Include(e => e.Track).ToListAsync();
            return enrollments.Select(e => new EnrollmentViewModel
            {
                Id = e.Id,
                InternName = e.Intern.FullName,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status.ToString()
            });
        }
    }
}