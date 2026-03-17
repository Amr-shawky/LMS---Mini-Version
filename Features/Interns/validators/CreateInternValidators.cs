using LMS___Mini_Version.Features.Interns.Commands.CreateInternCommand;
using LMS___Mini_Version.Infrastructure.Repositories;

namespace LMS___Mini_Version.Features.Interns.validators;

public class CreateInternValidators
{
    private readonly UnitOfWork _uow;
    public CreateInternValidators(UnitOfWork uow) => _uow = uow;

    public async Task<string?> ValidateAsync(CreateInternCommand command)
    {
        // Email check 
        var isEmailTaken = await _uow.Interns.IsEmailAddressTakenAsync(command.Email).ConfigureAwait(false);
        if (isEmailTaken) return $"Email {command.Email} is already taken";
        
        // Track check 
        var track = await _uow.Tracks.GetByIdAsync(command.TrackId).ConfigureAwait(false);
        if (track == null) return $"Track with Id {command.TrackId} Not found";
        
        return null;
    }
}