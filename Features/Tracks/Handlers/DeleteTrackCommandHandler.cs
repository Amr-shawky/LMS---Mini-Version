using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LMS___Mini_Version.Features.Tracks.Handlers;

public class DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepositry,
                                        IGeneralRepository<Enrollment> enrollmentRepositry,
                                      IUnitOfWork unitOfWork) : IRequestHandler<DeleteTrackCommand, bool>
{
    public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
    {
        var track = await trackRepositry.GetByIdAsync(request.Id);
        if (track == null) return false;

        var hasEnrollments = await enrollmentRepositry.
                              GetTable()
                              .AnyAsync(e => e.TrackId == request.Id);
        if(hasEnrollments) return false;
        trackRepositry.Delete(track);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
