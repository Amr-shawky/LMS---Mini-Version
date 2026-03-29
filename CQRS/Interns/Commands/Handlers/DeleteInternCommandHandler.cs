using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, bool>
    {
        private readonly IInternService _internService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(IInternService internService, IUnitOfWork unitOfWork)
        {
            _internService = internService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _internService.DeleteAsync(request.id);
            if (!deleted) return false;

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
