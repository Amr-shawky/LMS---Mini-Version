using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record UpdateTrackCommand(int Id, string Name, decimal Fees, bool IsActive, int MaxCapacity) : IRequest;

    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand,Unit>
    {
        private readonly IGeneralRepository<Track> _trackrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
            
        public UpdateTrackCommandHandler(IGeneralRepository<Track> trackrepository,IUnitOfWork unitOfWork,IMediator mediator)
        {
            _trackrepository = trackrepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            // use Update method not effieniant to update , it update all columns that chage value and not chage 
            //best practice to used the bluck update to get the entity and update in a i call of db 

            var trackExist = await _mediator.Send(new CheckTrackExistsQuery(request.Id) ,cancellationToken);
            if (!trackExist)
                throw new KeyNotFoundException($"Track not found.");
           

            var track = new Track
            {
               Name = request.Name,
               Fees = request.Fees,
               IsActive = request.IsActive,
               MaxCapacity = request.MaxCapacity
            };


            track.Name = request.Name;
            track.Fees = request.Fees;
            track.IsActive = request.IsActive;
            track.MaxCapacity = request.MaxCapacity;

            _trackrepository.Update(track);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
