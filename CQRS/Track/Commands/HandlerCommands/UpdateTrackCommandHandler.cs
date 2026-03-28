using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.CQRS.Track.Queries;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Track.Commands.HandlerCommands
{
    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.InvalidData);
            var existingTrack = await _mediator.Send(new GetTrackbyIdQuery(request.id), cancellationToken);
            if (existingTrack == null)
                return RequestResult<bool>.Failure(ErrorCode.NotExist);
            _uow.Tracks.Update(new Domain.Entities.Track
            {
                Id = request.id,
                Name = request.name??existingTrack.Result.Name,
                Fees = request.fees??existingTrack.Result.Fees,
                MaxCapacity=request.maxCapacity??existingTrack.Result.MaxCapacity,
                IsActive = request.isActive
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.InvalidData);
        }
    }
}
