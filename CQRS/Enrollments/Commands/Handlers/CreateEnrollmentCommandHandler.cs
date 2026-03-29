using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Handlers
{
    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResultDto>
    {
        private readonly EnrollInternMediator _enrollMediator;

        public CreateEnrollmentCommandHandler(EnrollInternMediator enrollMediator)
        {
            _enrollMediator = enrollMediator;
        }

        public async Task<EnrollmentResultDto> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var dto = new CreateEnrollmentDto
            {
                InternId = request.internId,
                TrackId = request.trackId
            };

            return await _enrollMediator.ExecuteAsync(dto);
        }
    }
}
