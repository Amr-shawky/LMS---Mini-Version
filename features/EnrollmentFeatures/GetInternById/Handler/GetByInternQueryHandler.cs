using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.DTOs;
using LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.Handler;

public class GetByInternQueryHandler(IGeneralRepository<Enrollment>  repository)
    : IRequestHandler<GetByInternQuery, RequestResponse<IEnumerable<EnrollmentDTO>>>
{
    public async Task<RequestResponse<IEnumerable<EnrollmentDTO>>> Handle(GetByInternQuery request, CancellationToken cancellationToken)
    {
        //check intern id
        var exist = await repository.GetAll().
            AnyAsync(x=>x.InternId == request.internId,cancellationToken);

        if (!exist)
        {
            return RequestResponse<IEnumerable<EnrollmentDTO>>.Fail(404, new Dictionary<string, string[]>
            {
                ["InternNotfount"] = new[] { "intern not found" }
            });
        }
        var internsInEnrollment = await repository.GetAll()
            .Where(x => x.InternId == request.internId).
            OrderByDescending(x => x.EnrollmentDate).
            Select(x => new EnrollmentDTO
            {
                EnrollmentId = x.Id,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.Status,
                TrackId = x.TrackId,
                TrackName = x.Track.Name

            })
            .ToListAsync();
        return RequestResponse<IEnumerable<EnrollmentDTO>>.OK(internsInEnrollment, 200);
            }
}
