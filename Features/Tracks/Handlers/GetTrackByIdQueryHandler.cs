using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackDto?>
    {
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IMediator _mediator;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepo,IMediator mediator)
        {
            _trackRepo = trackRepo;
            _mediator = mediator;
        }

        public async Task<TrackDto?> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track =await _trackRepo.GetByIdAsync(request.Id);
            if(track is null)
                return null;
            var currentEnrollmentCount=await _mediator.Send(new GetEnrollmentsCountByTrackIdQuery() { TrackId = track.Id } );
            return new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Fees = track.Fees,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                CurrentEnrollmentCount = currentEnrollmentCount
            };
        }
    }
}
