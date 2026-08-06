using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands;

public class UpdateInternCommandHandler (IGeneralRepository<Domain.Entities.Intern> _internRepo) 
    : IRequestHandler<UpdateInternCommand, bool>
{    
    public async Task<bool> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
    {
        var intern = await _internRepo.GetTable()
            .Where(i => i.Id == request.id)
            .FirstOrDefaultAsync(cancellationToken);
        if (intern == null) 
        {
            return false;
        }

        intern.FullName = request.fullName;
        intern.Email = request.email;
        intern.BirthYear = request.birthYear;
        intern.Status = request.state;
        intern.TrackId = request.trackId;

        _internRepo.Update(intern);

        return true;
    }
}
