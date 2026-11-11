using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.handler
{
    public class AddInternCommandHandler : IRequestHandler<AddInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Track> trackRepository;
        private readonly IGeneralRepository<Intern> internRepository;
        private readonly IUnitOfWork unitOfWork;

        public AddInternCommandHandler(IGeneralRepository<Track> _trackRepository, IGeneralRepository<Intern> internRepository , IUnitOfWork unitOfWork)
        {
            trackRepository = _trackRepository;
            this.internRepository = internRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<int>> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            var errors = ValidateRequest(request);
            if (errors is not null)
                return RequestResponse<int>.Fail(StatusCodes.Status400BadRequest, errors);

            var fullName = request.fullName.Trim();
            var email = request.email.Trim();
            var normalizedEmail = email.ToLower();

            var avaliableTrack = await trackRepository.GetAll().Where(x => x.Id == request.TrackId).Select(x => new
            {
                x.Id,
                x.IsActive
            }).FirstOrDefaultAsync(cancellationToken);

            if (avaliableTrack is null)
            {
                errors?.Add("TrackId", new[] { $"Track Id {request.TrackId} is not found" });
                return RequestResponse<int>.Fail(StatusCodes.Status404NotFound,errors  );
            }
            if (avaliableTrack is null)
            {
                errors?.Add("TrackId", new[] { $"Track with id {request.TrackId} was not found." });
                return RequestResponse<int>.Fail(StatusCodes.Status404NotFound,errors);
            }
            if (!avaliableTrack.IsActive)
            {
                errors?.Add("TrackId",new[] { "This track is not active and cannot accept new applicants."});
                return RequestResponse<int>.Fail(StatusCodes.Status409Conflict,
                   errors);
            }
            var emailTaken = await internRepository.GetAll()
                .AnyAsync(i => i.Email.ToLower() == normalizedEmail, cancellationToken);

            if (emailTaken)
            {
                errors?.Add("Email",new[] {"An intern with this email is already registered."});
                return RequestResponse<int>.Fail(StatusCodes.Status409Conflict,
                   errors);
            }
            var intern = new Intern
            {
                FullName = fullName,
                Email = email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = InternStatus.pending
            };

            internRepository.Add(intern);
            await unitOfWork.SaveChangesAsync();

            return RequestResponse<int>.OK(intern.Id, StatusCodes.Status201Created);
        }
        private static IDictionary<string, string[]> ValidateRequest(AddInternCommand request)
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.fullName))
                errors.Add(nameof(request.fullName), new[] { "Full name is required" });
            if (string.IsNullOrWhiteSpace(request.email))
                errors["email"] = new[] { "Email is required" };
            if (request.BirthYear <= 0)
                errors.Add(nameof(request.BirthYear), new[] { "Birth year must be a positive integer" });
            if (request.TrackId <= 0)
                errors.Add(nameof(request.TrackId), new[] { "Track ID must be a positive integer." });
            return errors;
        }
    }
}
