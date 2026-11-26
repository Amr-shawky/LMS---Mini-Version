using System.Net.Mail;
using LMS___Mini_Version.Domain.Constants;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeature.AddIntern.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.handler
{
    public class AddInternCommandHandler : IRequestHandler<AddInternCommand, RequestResponse<CreatedInternDto>>
    {
        private const int MinBirthYear = 1900;

        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddInternCommandHandler(
            IGeneralRepository<Intern> internRepository,
            IGeneralRepository<Track> trackRepository,
            IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<CreatedInternDto>> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            var fullName = request.FullName?.Trim() ?? string.Empty;
            var email = request.Email?.Trim() ?? string.Empty;

            var validationErrors = Validate(fullName, email, request.BirthYear, request.TrackId);
            if (validationErrors.Count > 0)
                return RequestResponse<CreatedInternDto>.Fail(StatusCodes.Status400BadRequest, validationErrors);

            var track = await _trackRepository.GetAll()
                .Where(t => t.Id == request.TrackId)
                .Select(t => new { t.Id, t.Name, t.IsActive })
                .FirstOrDefaultAsync(cancellationToken);

            if (track is null)
                return RequestResponse<CreatedInternDto>.Fail(StatusCodes.Status404NotFound,
                    nameof(request.TrackId), $"Track with id {request.TrackId} was not found.");

            if (!track.IsActive)
                return RequestResponse<CreatedInternDto>.Fail(StatusCodes.Status409Conflict,
                    nameof(request.TrackId), $"Track '{track.Name}' is not active and cannot accept new applicants.");

            // Case-insensitive comparison regardless of the database collation.
            var normalizedEmail = email.ToLower();
            var emailTaken = await _internRepository.GetAll()
                .AnyAsync(i => i.Email.ToLower() == normalizedEmail, cancellationToken);

            if (emailTaken)
                return RequestResponse<CreatedInternDto>.Fail(StatusCodes.Status409Conflict,
                    nameof(request.Email), $"An intern with the email '{email}' is already registered.");

            var intern = new Intern
            {
                FullName = fullName,
                Email = email,
                BirthYear = request.BirthYear,
                TrackId = track.Id,
                Status = InternStatuses.Pending
            };

            _internRepository.Add(intern);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse<CreatedInternDto>.OK(new CreatedInternDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email = intern.Email,
                Status = intern.Status,
                TrackId = track.Id,
                TrackName = track.Name
            }, StatusCodes.Status201Created);
        }

        private static Dictionary<string, string[]> Validate(string fullName, string email, int birthYear, int trackId)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors[nameof(AddInternCommand.FullName)] = new[] { "Full name is required." };

            if (string.IsNullOrWhiteSpace(email))
                errors[nameof(AddInternCommand.Email)] = new[] { "Email is required." };
            else if (!MailAddress.TryCreate(email, out var parsed) || parsed.Address != email)
                errors[nameof(AddInternCommand.Email)] = new[] { $"'{email}' is not a valid email address." };

            var currentYear = DateTime.UtcNow.Year;
            if (birthYear < MinBirthYear || birthYear > currentYear)
                errors[nameof(AddInternCommand.BirthYear)] = new[] { $"Birth year must be between {MinBirthYear} and {currentYear}." };

            if (trackId <= 0)
                errors[nameof(AddInternCommand.TrackId)] = new[] { "A valid track id is required." };

            return errors;
        }
    }
}
