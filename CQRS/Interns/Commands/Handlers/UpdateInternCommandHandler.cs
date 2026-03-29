using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.Handlers
{
    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, bool>
    {
        private readonly IInternService _internService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInternCommandHandler(IInternService internService, IUnitOfWork unitOfWork)
        {
            _internService = internService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var dto = new InternDto
            {
                FullName = request.fullName,
                Email = request.email,
                BirthYear = request.birthYear,
                Status = request.status,
                TrackId = request.trackId
            };

            var updated = await _internService.UpdateAsync(request.id, dto);
            if (!updated) return false;

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
