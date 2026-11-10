using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.DTOS;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.Quey;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.Handler
{
    public class GetEnrollmentTrackInternHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetEnrollmentTrackInternQuery, RequestResponse<IEnumerable<GetEnrollmentTrackInternDTO>>>
    {
        public async Task<RequestResponse<IEnumerable<GetEnrollmentTrackInternDTO>>> Handle(GetEnrollmentTrackInternQuery request, CancellationToken cancellationToken)
        {
            int trackId = request.TrackId;
            var track = await unitOfWork.GetRepository<Track>().GetByIdAsync(trackId);
            if (track == null)
                return RequestResponse<IEnumerable<GetEnrollmentTrackInternDTO>>.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["TrackId"] = ["Track not found"] });
            var Enrollment = await unitOfWork.GetRepository<Enrollment>().GetAll()
                .Where(w => w.TrackId == trackId)
                .Select(s => new GetEnrollmentTrackInternDTO
                (s.Id,
                s.Intern.FullName,
                s.EnrollmentDate.ToString("yyyy-MM-dd"),
                s.Status.ToString(),
                s.Payment!.Amount, s.Payment.Status.ToString())
                ).ToListAsync(cancellationToken);
            return RequestResponse<IEnumerable<GetEnrollmentTrackInternDTO>>.OK(Enrollment);
        }
    }
}
