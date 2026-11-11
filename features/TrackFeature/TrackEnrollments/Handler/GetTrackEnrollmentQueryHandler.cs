using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackEnrollments.DTOs;
using LMS___Mini_Version.features.TrackFeature.TrackEnrollments.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.TrackEnrollments.Handler;

public class GetTrackEnrollmentQueryHandler(
    IGeneralRepository<Track> trackRepository,
    IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<List<TrackEnrollmentDto>>>
{
    public async Task<RequestResponse<List<TrackEnrollmentDto>>> Handle(
        GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        // track must exist 
        var track = await trackRepository.GetByIdAsync(request.TrackId);
        if (track is null)
            return RequestResponse<List<TrackEnrollmentDto>>.Fail(404, new Dictionary<string, string[]>
            {
                { "TrackId", ["Track not found"] }
            });


        var enrollments = await enrollmentRepository.GetAll()
            .Where(e => e.TrackId == request.TrackId)
            .OrderByDescending(e => e.EnrollmentDate)
            .ThenByDescending(e => e.Id)
            .Select(e => new TrackEnrollmentDto(
                e.Id,
                e.Intern.FullName,
                e.EnrollmentDate,
                e.Status.ToString(),
                e.Payment != null ? e.Payment.Amount : null,
                e.Payment != null ? e.Payment.Status.ToString() : null))
            .ToListAsync(cancellationToken);

        return RequestResponse<List<TrackEnrollmentDto>>.OK(enrollments);
    }
}