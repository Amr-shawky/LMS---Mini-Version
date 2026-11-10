using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Handlers
{
    public class GetInternEnrollmentHistoryQueryHandler : IRequestHandler<GetInternEnrollmentHistoryQuery, RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>
    {
        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetInternEnrollmentHistoryQueryHandler(IMediator mediator, IGeneralRepository<Enrollment> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>> Handle(GetInternEnrollmentHistoryQuery request, CancellationToken cancellationToken)
        {
            var intern = await _mediator.Send(new GetInternByIdQuery(request.internId));
            
            if(!intern.Success)
                return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.Fail(404, "InternId", "Intern Not Found");

            var enrollments = await _repository.GetAll()
            .Where(e => e.InternId == request.internId)
            .OrderByDescending(e => e.EnrollmentDate)
            .Select(e => new GetInternEnrollmentHistoryDto
            {
                EnrollmentId = e.Id,
                TrackId = e.TrackId,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                EnrollmentStatus = e.Status.ToString(),
            }).ToListAsync();

            return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.OK(enrollments);
        }
    }
}
