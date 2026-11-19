using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Dtos;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Queries
{
    public record GetTrackByIDQuery(int id) : IRequest<RequestResponse<TrackDto>>;

}
