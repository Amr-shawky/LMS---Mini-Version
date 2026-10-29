using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, RequestResponse<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepository;

        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepository = enrollmentrepo;
        }
        public async Task<RequestResponse<EnrollmentDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollmentDto = await _enrollmentrepository.GetAll()
                .Where(e => e.Id == request.EnrollmentId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    InternName = e.Intern != null ? e.Intern.FullName: string.Empty,
                    TrackId = e.TrackId,
                    TrackName = e.Track != null? e.Track.Name: string.Empty,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                })
                .FirstOrDefaultAsync(cancellationToken);
           
            if (enrollmentDto== null)
                return RequestResponse<EnrollmentDto>.Fail($"Enrollment with ID {request.EnrollmentId} not found", 404);
            
            return RequestResponse<EnrollmentDto>.Ok(enrollmentDto);
        }
    }
}
