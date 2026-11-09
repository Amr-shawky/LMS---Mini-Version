using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Command.Create;

public class CreateInternCommandHandler(
    IUnitOfWork unitOfWork,
    IGeneralRepository<Intern> internRepository,
    IGeneralRepository<Track> trackRepository)
    : IRequestHandler<CreateInternCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {
        // get track by id
        var track = await trackRepository.GetByIdAsync(request.TrackId);
        if (track is null)
            return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
            {
                { "TrackId", ["Track not found"] }
            });

        // check if track is active
        if (!track.IsActive)
            return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
            {
                { "TrackId", ["Track is not active"] }
            });

        // check if email already exists (case-insensitive)
        var email = request.Email.Trim().ToLower();
        var emailExist = await internRepository.GetAll()
            .AnyAsync(i => i.Email.ToLower() == email, cancellationToken);
        if (emailExist)
            return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
            {
                { "Email", ["Email is already registered."] }
            });

        // build intern object
        var intern = new Intern
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            BirthYear = request.BirthYear,
            TrackId = request.TrackId,
            Status = "Pending"
        };

        // create intern
        internRepository.Add(intern);
        await unitOfWork.SaveChangesAsync();

        return RequestResponse<int>.OK(intern.Id, 201);
    }
}