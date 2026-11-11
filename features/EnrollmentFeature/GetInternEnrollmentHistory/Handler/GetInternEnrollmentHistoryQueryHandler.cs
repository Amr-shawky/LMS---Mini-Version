using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Query;
using LMS___Mini_Version.features.InternFeature.GetInternById.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Handler
{
    public class GetInternEnrollmentHistoryQueryHandler : IRequestHandler<GetInternEnrollmentHistoryQuery, RequestResponse<IEnumerable<EnrollmentHistoryDto>>>
    {
        private readonly IGeneralRepository<Enrollment> repository;
        private readonly ISender sender;

        public GetInternEnrollmentHistoryQueryHandler(IGeneralRepository<Enrollment> repository , ISender sender)
        {
            this.repository = repository;
            this.sender = sender;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentHistoryDto>>> Handle(GetInternEnrollmentHistoryQuery request, CancellationToken cancellationToken)
        {
            var Intern = await sender.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
            if(!Intern.Success|| Intern.Data is null)
            {
                return RequestResponse<IEnumerable<EnrollmentHistoryDto>>.Fail(404,new Dictionary<string, string[]>
                {
                      ["Intern"]= new[] { "Intern not found." }
                });
            }

            var Enrollments =await repository.GetAll().Where(e => e.InternId == request.InternId).OrderByDescending(e => e.EnrollmentDate).Select(e => new EnrollmentHistoryDto
            {
                EnrollmentId = e.Id,
                TrackId = e.TrackId,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                enrollmentStatus = e.Status
            }).ToListAsync(cancellationToken);
            return RequestResponse<IEnumerable<EnrollmentHistoryDto>>.OK(Enrollments);
        }
    }
}
