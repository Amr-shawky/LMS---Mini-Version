using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;
using LMS___Mini_Version.CQRS.Enrollments.Services;
using LMS___Mini_Version.CQRS.Enrollments.Validators;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Handlers
{
    public class EnrollmentInternCommandHandler : IRequestHandler<EnrollmentInternCommand, EnrollmentResultDto>
    {
        private readonly IUnitOfWork _unitOf;
        private readonly EnrollmentValidator _validator;
        private readonly EnrollmentCreator _enrollmentCreator;
        public EnrollmentInternCommandHandler(EnrollmentCreator enrollmentCreator, EnrollmentValidator validator, IUnitOfWork unitOf)
        {
            _enrollmentCreator = enrollmentCreator;
            _validator = validator;
            _unitOf = unitOf;
        }

        public async Task<EnrollmentResultDto> Handle(EnrollmentInternCommand request, CancellationToken cancellationToken)
        {
            var error = await _validator.ValidateAsync(request);
            if (error != null)
                return EnrollmentResultDto.Fail(error);

            var track = await _unitOf.Tracks.GetByIdAsync(request.TrackId);
            return await _enrollmentCreator.CreateAsync(request, track);
        }
    }
}

