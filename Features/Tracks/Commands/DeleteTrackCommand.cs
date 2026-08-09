using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record DeleteTrackCommand(int Id) : IRequest;


    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand , Unit>
    {
        private readonly IGeneralRepository<Track> _trackrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackrepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _trackrepository = trackrepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }


        public async Task<Unit> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            // the correct way donot getbyId it get the full entity and the delete it occures by id not full entity 

            var track = await _mediator.Send(new GetTrackByIdQuery(request.Id), cancellationToken);
            if (track == null)
                throw new KeyNotFoundException($"Track not found.");

            var trackEntity = new Track { Id = request.Id }; 

            _trackrepository.Delete(trackEntity);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
