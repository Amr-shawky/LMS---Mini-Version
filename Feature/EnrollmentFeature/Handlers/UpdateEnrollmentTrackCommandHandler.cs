using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Handlers
{
    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public Task<RequestResponse> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            // Create a new Enrollment entity with the updated TrackId 
            var entity = new Enrollment
            {
                Id = request.EnrollmentId,
                TrackId = request.NewTrackId
            };
            // Save the updated entity to the database
            _enrollmentRepository.SaveInclude(entity, nameof(Enrollment.TrackId));
            return Task.FromResult(RequestResponse.Ok("Enrollment track updated successfully."));
        }
    }
}
