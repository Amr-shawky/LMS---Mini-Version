namespace LMS___Mini.CQRS.Interns.Validators
{
    public class CreateInternValidators
    {
        private readonly IUnitOfWork _unitOf;
        public CreateInternValidators(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }
        public async Task<string?> ValidateAsync(CreateInternCommand command)
        {
            var isEmailTaken = await _unitOf.Interns.IsEmailAddressTakenAsync(command.Email);
            if (isEmailTaken) return $"Email {command.Email} is already taken";

            var track = await _unitOf.Tracks.GetByIdAsync(command.TrackId);
            if (track == null) return $"Track with Id {command.TrackId} Not found";

            return null;
        }
    }
}