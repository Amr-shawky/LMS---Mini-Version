using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Query
{
    public record GetTrackByIdQuery(int id): IRequest<RequestResponse<GetTrackByIdDto>>
    {
    }
}
