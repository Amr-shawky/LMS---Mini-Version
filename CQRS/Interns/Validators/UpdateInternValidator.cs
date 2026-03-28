namespace LMS___Mini.CQRS.Interns.Validators
{
    public class UpdateInternValidator
    {
        private readonly IUnitOfWork _unitOf;
        public UpdateInternValidator(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<string?> ValidateAsync(UpdateInternCommand command)
        {
            var intern = await _unitOf.Interns.GetByIdAsync(command.InternId);
            if (intern == null) return $"Intern with Id {command.InternId} Doesn't exist";

            if (intern.Email != command.Email)
            {
                var isEmailTaken = await _unitOf.Interns.IsEmailAddressTakenAsync(command.Email);
                if (isEmailTaken) return $"Email {command.Email} is already taken";
            }

            return null;
        }


    }
}
