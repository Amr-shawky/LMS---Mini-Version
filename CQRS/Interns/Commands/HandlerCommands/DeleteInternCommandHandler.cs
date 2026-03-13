using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands;

public class DeleteInternCommandHandler(IGeneralRepository<Domain.Entities.Intern> _internRepo) 
    : IRequestHandler<DeleteInternCommand, bool>
{
   
    public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {

        var intern = await _internRepo.GetTable()
            .Where(i => i.Id == request.id).FirstOrDefaultAsync(cancellationToken);

        if (intern == null) return false;

        _internRepo.Delete(intern);

         //await _unitOfWork.CompleteAsync().ConfigureAwait(false);
        return true;
    }
}
