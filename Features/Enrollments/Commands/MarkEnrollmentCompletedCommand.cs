using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record MarkEnrollmentCompletedCommand(int enrollmentId) : IRequest<RequestResponse>;

    public class MarkEnrollmentCompletedCommandHandler : IRequestHandler<MarkEnrollmentCompletedCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public MarkEnrollmentCompletedCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse> Handle(MarkEnrollmentCompletedCommand command,CancellationToken cancellationToken)
        {
            var enrollment = _enrollmentRepo.GetById(command.enrollmentId);

            enrollment.Status = EnrollmentStatus.Completed;

            _enrollmentRepo.Update(enrollment);

            return RequestResponse.OK();
        }
    }
}
