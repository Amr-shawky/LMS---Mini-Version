using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Handler
{
    public class GetAllEnrollmentsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetAllEnrollmentQuery, RequestResponse<List<GetAllEnrollmentsDTO>>>
    {
        public async Task<RequestResponse<List<GetAllEnrollmentsDTO>>> Handle(
            GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var intern = await unitOfWork.GetRepository<Intern>().GetByIdAsync(request.InternId);

            if (intern is null)
                return RequestResponse<List<GetAllEnrollmentsDTO>>.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["InternId"] = [$"Intern with Id {request.InternId} was not found."] });

            //var Enrollments = unitOfWork.GetRepository<Enrollment>().GetAll()
            //    //All Interns assigned to Enrollments
            //    .Where(e => e.InternId == request.InternId)
            //    //date
            //    .OrderByDescending(e => e.EnrollmentDate)
            //    .Select(e => new GetAllEnrollmentsDTO(e.Id, e.TrackId, e.EnrollmentDate.ToShortDateString(), e.Status.ToString(), e.Track.Name))
            //    .ToList();
            //return RequestResponse<List<GetAllEnrollmentsDTO>>.OK(Enrollments);

            var rows = await unitOfWork.GetRepository<Enrollment>().GetAll()
    .Where(e => e.InternId == request.InternId)
    .OrderByDescending(e => e.EnrollmentDate)
    .ThenByDescending(e => e.Id)
    .Select(e => new { e.Id, e.TrackId, e.EnrollmentDate, e.Status, TrackName = e.Track.Name })
    .ToListAsync(cancellationToken);

            var enrollments = rows
                .Select(r => new GetAllEnrollmentsDTO(
                    r.Id, r.TrackId, r.EnrollmentDate.ToString("yyyy-MM-dd"), r.Status.ToString(), r.TrackName))
                .ToList();

            return RequestResponse<List<GetAllEnrollmentsDTO>>.OK(enrollments, StatusCodes.Status200OK);

        }
    }
}