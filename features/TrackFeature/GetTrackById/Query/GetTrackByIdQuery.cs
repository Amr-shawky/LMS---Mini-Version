using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Query
{
    public record GetTrackByIdQuery(int Id)
       : IRequest<RequestResponse<TrackDtoById>>;
}
