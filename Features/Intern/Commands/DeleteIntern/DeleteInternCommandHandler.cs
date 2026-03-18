using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.DeleteIntern
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(request.id).ConfigureAwait(false);
            if (intern == null) return false;

            _unitOfWork.Interns.Delete(intern);
            return true;
        }
    }
}
