using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.CQRS.Track.Queries;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Track.Commands.HandlerCommands
{
    public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingTrack = await _mediator.Send(new IsTrackNameExistQuery (request.name), cancellationToken);
            if(existingTrack.Result==true)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            _uow.Tracks.Add(new Domain.Entities.Track
            {
                Name = request.name,
                Fees = request.fees,
                IsActive = request.isActive,
                MaxCapacity = request.maxCapacity
            });
            return await _uow.CompleteAsync() > 0 
                ? RequestResult<bool>.Success(true) 
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
