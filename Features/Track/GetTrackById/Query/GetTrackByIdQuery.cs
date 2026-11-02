using LMS___Mini_Version.Common;
using LMS___Mini_Version.Features.Track.GetTrackById.Dtos;
using MediatR;

namespace LMS___Mini_Version.Features.Track.GetTrackById.Query
{
    public record GetTrackByIdQuery(int trackId) : IRequest<GetTrackDto>;
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, GetTrackDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTrackByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetTrackDto> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
         var track = await  _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Track, int>().GetByIdAsync(request.trackId);
            if (track == null)
            {
                return null;
            }
            else
            {
                var trackDto = new GetTrackDto()
                {
                    Fees = track.Fees,
                    IsActive = track.IsActive,
                    Name = track.Name
                };

                return trackDto;
            }
        }
    }
}
