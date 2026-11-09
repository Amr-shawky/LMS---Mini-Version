using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.GetInternEnrollments;

public class GetInternEnrollmentsQueryHandler(
    IGeneralRepository<Intern> internRepository,
    IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<GetInternEnrollmentsQuery, RequestResponse<List<EnrollmentDto>>>
{
    public async Task<RequestResponse<List<EnrollmentDto>>> Handle(
        GetInternEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        // check if intern exist
        var intern = await internRepository.GetByIdAsync(request.InternId);
        if (intern is null)
            return RequestResponse<List<EnrollmentDto>>.Fail(404, new Dictionary<string, string[]>
            {
                { "InternId", ["Intern not found"] }
            });
        
        // get enrollments

        var enrollments = await enrollmentRepository.GetAll()
            .Where(e => e.InternId == request.InternId)
            .OrderByDescending(e => e.EnrollmentDate)
            .Select(e => new EnrollmentDto(
                e.Id,
                e.TrackId,
                e.Track.Name,
                e.EnrollmentDate,
                e.Status.ToString()))
            .ToListAsync(cancellationToken);
        
        return RequestResponse<List<EnrollmentDto>>.OK(enrollments);
    }
}