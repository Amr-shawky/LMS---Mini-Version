using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record CreateEnrollmentCommand(int InternId, int TrackId) : IRequest<RequestResponse<EnrollmentDto>>;

    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, RequestResponse<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public CreateEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<EnrollmentDto>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
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

            var dto = new EnrollmentDto
            {
                Id = enrollment.Id,
                InternId = enrollment.InternId,
                TrackId = enrollment.TrackId,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status
            };

            return RequestResponse<EnrollmentDto>.Created(dto);
        }
    }
}
