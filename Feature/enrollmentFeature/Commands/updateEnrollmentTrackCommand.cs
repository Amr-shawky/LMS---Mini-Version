using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record updateEnrollmentTrackCommand(int EnrollmentID, int TrackID) : IRequest<RequestResponse<bool>>;

    public sealed class UpdateEnrollmentTrackCommandHandler : IRequestHandler<updateEnrollmentTrackCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> repository) => _repository = repository;

        public async Task<RequestResponse<bool>> Handle(updateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _repository.GetByIdAsync(request.EnrollmentID);
            if (enrollment is null)
                return RequestResponse<bool>.Fail($"Enrollment {request.EnrollmentID} was not found.", 404);

            enrollment.TrackId = request.TrackID;
            _repository.SaveInclude(enrollment, nameof(Enrollment.TrackId));
            return RequestResponse<bool>.Ok(true);
        }
    }
}
