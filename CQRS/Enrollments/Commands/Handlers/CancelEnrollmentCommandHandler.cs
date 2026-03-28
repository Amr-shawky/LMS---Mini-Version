using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;
using LMS___Mini_Version.CQRS.Enrollments.Services;
using LMS___Mini_Version.CQRS.Enrollments.Validators;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Handlers
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand, EnrollmentResultDto>
    {
        private readonly CancelEnrollmentValidator _validator;
        private readonly EnrollmentCanceller _canceller;

        public CancelEnrollmentCommandHandler(CancelEnrollmentValidator validator, EnrollmentCanceller canceller)
        {
            _validator = validator;
            _canceller = canceller;
        }

        public async Task<EnrollmentResultDto> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var error = await _validator.ValidateAsync(request);
            if (error != null)
                return EnrollmentResultDto.Fail(error);

            return await _canceller.CancelAsync(request.EnrollmentId);
        }
    }
}

