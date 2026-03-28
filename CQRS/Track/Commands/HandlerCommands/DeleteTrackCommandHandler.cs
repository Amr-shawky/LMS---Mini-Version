using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.CQRS.Track.Queries;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Track.Commands.HandlerCommands
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.InvalidData);
            var existingTrack = await _mediator.Send(new GetTrackbyIdQuery(request.id), cancellationToken);
            if (existingTrack == null)
                return RequestResult<bool>.Failure(ErrorCode.NotExist);
            _uow.Tracks.Delete(new Domain.Entities.Track { 
                Id = request.id,
                Name= existingTrack.Result.Name,
                Fees = existingTrack.Result.Fees,
                IsActive = existingTrack.Result.IsActive,
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.InvalidData);
        }
    }
}
