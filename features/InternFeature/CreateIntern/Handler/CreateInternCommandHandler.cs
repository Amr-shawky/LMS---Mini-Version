
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.Handler
{
    public class CreateInternCommandHandler
        : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeneralRepository<Intern> internRepo;
        private readonly ISender sender;

        public CreateInternCommandHandler(
            IUnitOfWork unitOfWork,
            IGeneralRepository<Intern> internRepo,
            ISender sender)
        {
            this.unitOfWork = unitOfWork;
            this.internRepo = internRepo;
            this.sender = sender;
        }

        public async Task<RequestResponse<int>> Handle(
            CreateInternCommand request,
            CancellationToken cancellationToken)
        {
            var track = await sender.Send(
                new GetTrackByIdQuery(request.TrackId),
                cancellationToken);

            if (!track.Success || track.Data is null)
            {
                return RequestResponse<int>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track not found." }
                    });
            }
            if (!track.Data.IsActive)
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[]
                        {
                            "Selected track is not active."
                        }
                    });
            }
            var email = request.Email.Trim().ToLowerInvariant();

            var emailExists = await internRepo.GetAll()
                .AnyAsync(
                    i => i.Email.ToLower() == email,
                    cancellationToken);

            if (emailExists)
            {
                return RequestResponse<int>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["Email"] = new[]
                        {
                            "Email is already registered."
                        }
                    });
            }
            var intern = new Intern
            {
                FullName = request.FullName.Trim(),
                Email = email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };
            internRepo.Add(intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(intern.Id, 201);
        }
    }
}
