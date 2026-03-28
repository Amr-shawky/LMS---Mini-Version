using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Track.Queries.HandlerQueries
{
    public class IsTrackNameExistQueryHandler : IRequestHandler<IsTrackNameExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public async Task<RequestResult<bool>> Handle(IsTrackNameExistQuery request, CancellationToken cancellationToken)
        {
            var track = await _uow.Tracks.GetTable().AnyAsync(track => track.Name == request.name);
            return RequestResult<bool>.Success(track);
        }
    }
}
