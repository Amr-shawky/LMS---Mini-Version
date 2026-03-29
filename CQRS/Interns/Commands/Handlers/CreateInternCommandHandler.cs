using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, InternDto>
    {
        private readonly IInternService _internService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInternCommandHandler(IInternService internService, IUnitOfWork unitOfWork)
        {
            _internService = internService;
            _unitOfWork = unitOfWork;
        }

        public async Task<InternDto> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var dto = new InternDto
            {
                FullName = request.fullName,
                Email = request.email,
                BirthYear = request.birthYear,
                Status = request.status,
                TrackId = request.trackId
            };

            var created = await _internService.CreateAsync(dto);
            await _unitOfWork.CompleteAsync();
            return created;
        }
    }
}
