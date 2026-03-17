using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.HandlerQueries
{
    public class GetEnrollmentByInternQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEnrollmentByInternQuery,IEnumerable<EnrollmentViewModel>>
    {
        public async Task<IEnumerable<EnrollmentViewModel>> Handle(GetEnrollmentByInternQuery request, CancellationToken cancellationToken)
        {
            var enrollments = _unitOfWork.Enrollments.GetTable().Where(e => e.InternId == request.InternId);

            var result = enrollments.Select(e => new EnrollmentViewModel{

                 Id = e.Id,
                InternName = e.Intern.FullName,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status.ToString()
            }).ToList();
            return result;
        }
    }
}
