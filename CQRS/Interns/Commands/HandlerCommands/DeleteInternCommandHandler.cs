using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.HandlerCommands
{
    public class DeleteInternCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteInternCommand, bool>
    {
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
           var intern = await _unitOfWork.Interns.GetByIdAsync(request.Id);
           if (intern == null) 
                return false;
             _unitOfWork.Interns.Delete(intern);
             await _unitOfWork.CompleteAsync();
             return true;
        }
    }
}
