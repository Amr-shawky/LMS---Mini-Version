using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.HandlerCommands
{
    public class UpdateInternCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateInternCommand, bool>
    {
        public async Task<bool> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Interns.GetByIdAsync(request.id);
            if(entity == null) 
                return false;
            entity.FullName = request.FullName;
            entity.Email = request.Email;
            entity.BirthYear = request.BirthYear;
            entity.Status = request.Status;
            entity.TrackId = request.TrackId;
            
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
