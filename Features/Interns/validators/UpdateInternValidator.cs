using LMS___Mini_Version.Features.Interns.Commands.UpdateInternCommand;
using LMS___Mini_Version.Infrastructure.Repositories;

namespace LMS___Mini_Version.Features.Interns.validators;

public class UpdateInternValidator
{
    private readonly UnitOfWork _uow;
    public UpdateInternValidator(UnitOfWork uow) => _uow = uow;
    
    public async Task<string?> ValidateAsync(UpdateInternCommand command)
    {
        var intern = await _uow.Interns.GetByIdAsync(command.InternId).ConfigureAwait(false);
        if (intern == null) return $"Intern with Id {command.InternId} Doesn't exist";

        if (intern.Email != command.Email)
        {
            var isEmailTaken = await _uow.Interns.IsEmailAddressTakenAsync(command.Email).ConfigureAwait(false);
            if (isEmailTaken) return $"Email {command.Email} is already taken";
        }

        return null;

    }
}