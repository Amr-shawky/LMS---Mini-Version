using FluentValidation;
using LMS___Mini_Version.ViewModels.Intern;

namespace LMS___Mini_Version.Validators
{
    public class UpdateInternValidator : AbstractValidator<UpdateInternViewModel>
    {
        public UpdateInternValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");

            RuleFor(x => x.BirthYear)
                .InclusiveBetween(1980, DateTime.UtcNow.Year - 16)
                .WithMessage("Birth year must be between 1980 and " + (DateTime.UtcNow.Year - 16) + ".");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.");

            RuleFor(x => x.TrackId)
                .GreaterThan(0).WithMessage("Track ID must be a positive number.");
        }
    }
}
