using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class AddInternCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddInternCommand, RequestResponse<int>>
{


    public async Task<RequestResponse<int>> Handle(AddInternCommand request, CancellationToken cancellationToken)
    {
        var internDto = request.InternDto;
        var internRepo = unitOfWork.GetRepository<Intern>();
        var trackRepo = unitOfWork.GetRepository<Track>();

        var track = await trackRepo.GetAll().FirstOrDefaultAsync(t => t.Id == internDto.TrackId, cancellationToken);

        if (track is null)
        {
            return RequestResponse<int>.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["TrackId"] = ["Track not found"] });
        }

        if (!track.IsActive)
        {
            return RequestResponse<int>.Fail(StatusCodes.Status409Conflict, new Dictionary<string, string[]> { ["TrackId"] = ["Track is not active and cannot accept new interns"] });
        }
        var emailExists = await internRepo.GetAll().AnyAsync(i => i.Email.ToLower() == internDto.Email.ToLower(), cancellationToken);
        if (emailExists)
            return RequestResponse<int>.Fail(StatusCodes.Status409Conflict, new Dictionary<string, string[]> { ["Email"] = ["An intern with this email already exists"] });

        var intern = new Intern
        {
            FullName = internDto.FullName,
            BirthYear = internDto.BirthYear,
            Email = internDto.Email,
            TrackId = internDto.TrackId,
            Status = InternStatus.Pending.ToString()
        };

        internRepo.Add(intern);
        //await unitOfWork.SaveChangesAsync();

        return RequestResponse<int>.OK(intern.Id, StatusCodes.Status201Created);
    }
}