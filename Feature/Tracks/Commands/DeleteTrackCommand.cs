using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Commands
{
    public record DeleteTrackCommand(int Id) : IRequest<RequestResponse>;

    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepository, IUnitOfWork unitOfWork)
        {
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetByIdAsync(request.Id);
            if (track == null)
            {
                return RequestResponse.Fail($"Track with ID {request.Id} not found", 404);
            }

            _trackRepository.Delete(track);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.Ok("Track deleted successfully");
        }
    }
}
