using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentsFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentsFeature.Queries;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentsFeature.Handler
{
    public class GetInternEnrollmentHistoryQueryHandler : IRequestHandler<GetInternEnrollmentHistoryQuery, RequestResponse<IEnumerable<EnrollmentHistoryDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IMediator _mediator;
        public GetInternEnrollmentHistoryQueryHandler(IGeneralRepository<Enrollment> repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        public async Task<RequestResponse<IEnumerable<EnrollmentHistoryDto>>> Handle(GetInternEnrollmentHistoryQuery request, CancellationToken cancellationToken)
        {
            // 1. Check if Intern exists
            var internExists = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);

            if (!internExists.Success)
            {
                return RequestResponse<IEnumerable<EnrollmentHistoryDto>>.Fail(internExists.StatusCode, internExists.Errors);
            }

            // 2. Get all enrollments with their associated tracks
            var enrollments = await _repository.GetAll().Select(e => new EnrollmentHistoryDto()
            {
                EnrollmentId = e.Id,
                TrackId = e.TrackId,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                EnrollmentStatus = e.Status,
            }).ToListAsync();
            // 3. Return result
            return RequestResponse<IEnumerable<EnrollmentHistoryDto>>.OK(enrollments);
        }
    }
}
