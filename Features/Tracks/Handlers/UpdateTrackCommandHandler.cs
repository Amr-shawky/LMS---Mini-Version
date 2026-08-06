using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;
using System.Diagnostics;

namespace LMS___Mini_Version.Features.Tracks.Handlers;

public class UpdateTrackCommandHandler(IGeneralRepository<Track> trackRepositry,IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateTrackCommand, bool>
{
    public async Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {
        var track =await trackRepositry.GetByIdAsync(request.Id);
        if(track == null)
            return false;

        track.Name = request.Name;
        track.Fees = request.Fees;
        track.IsActive = request.IsActive;
        track.MaxCapacity = request.MaxCapacity;

        trackRepositry.Update(track);
        await unitOfWork.CompleteAsync();

        return true;
    }
}
