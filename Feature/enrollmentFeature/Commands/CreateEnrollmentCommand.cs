using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var duplicate = await _enrollmentRepository.GetAll()
                .AnyAsync(e => e.InternId == request.InternId && e.TrackId == request.TrackId && e.Status != EnrollmentStatus.Cancelled, cancellationToken);

            if (duplicate)
            {
                return RequestResponse<int>.Fail("The intern is already enrolled in this track.", 400);
            }

            var enrollment = new Enrollment
            {
                InternId = request.InternId,
                TrackId = request.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            _enrollmentRepository.Add(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return RequestResponse<int>.Created(enrollment.Id);
        }
    }
}
