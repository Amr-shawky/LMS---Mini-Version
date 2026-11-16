using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.DTOs;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Handlers;

public class GetTrackEnrollmentsQueryHandlers(
    IGeneralRepository<Enrollment> repository,
    IGeneralRepository<Track> repo) :
    IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentDTO>>>
{
    

    async Task<RequestResponse<IEnumerable<TrackEnrollmentDTO>>> IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentDTO>>>.Handle(GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var trackExists = await repo.GetAll().AnyAsync(x => x.Id == request.id);

        if (!trackExists)
        {
            return RequestResponse<IEnumerable<TrackEnrollmentDTO>>.Fail(404,
                new Dictionary<string, string[]>
                {
                    ["TrackId"] = new[] { "Track not found." }
                });

        }

        var EnrollmentDtos  = await repository.GetAll()
            .Where(x=>x.TrackId == request.id).
            Select(x=> new TrackEnrollmentDTO 
            { 
                InternFullName= x.Intern.FullName,
                EnrollmentDate= x.EnrollmentDate,
                EnrollmentId= x.Id,
                EnrollmentStatus = x.Status,
                PaymentAmount = x.Payment !=null ? x.Payment.Amount : null ,
                PaymentStatus = x.Payment != null ? x.Payment.Status : null
            } ).ToListAsync(); // tolist hadn;t return null 


      


        return RequestResponse<IEnumerable<TrackEnrollmentDTO>>.OK(EnrollmentDtos, 200);

    }
}
