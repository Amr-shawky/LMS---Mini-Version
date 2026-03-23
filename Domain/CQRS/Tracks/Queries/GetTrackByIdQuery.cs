using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Tracks.Queries
{
    public record GetTrackByIdQuery(int Id) : IRequest<TrackDto>;
   
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackDto>
    {
        private readonly IGeneralRepository<Track> _repository;
        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<TrackDto> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetById(request.Id);
            //if (track == null)
            //{
            //    throw new NotFoundException($"Track with ID {request.Id} not found.");
            //}
            //return new TrackDto
            //{
            //    Id = track.Id,
            //    Name = track.Name,
            //    Description = track.Description,
            //    Duration = track.Duration
            //};
                return new TrackDto();
        }
    }
}
