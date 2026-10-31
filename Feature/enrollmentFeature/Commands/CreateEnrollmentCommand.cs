using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record CreateEnrollmentCommand(int InternId, int TrackId) : IRequest<RequestResponse<int>>;

    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public CreateEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<int>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = new Enrollment
            {
                InternId = request.InternId,
                TrackId = request.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            _enrollmentRepository.Add(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            // Return only the new ID. The orchestrator is responsible for any further work
            // (e.g. creating a payment). Callers that need full data can issue a separate query.
            return RequestResponse<int>.Created(enrollment.Id);
        }
    }
}
