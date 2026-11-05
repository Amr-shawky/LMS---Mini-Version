using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Dtos;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Queries
{
    public record GetAllTracksQuery(int pageindex, int pagesize) : IRequest<RequestResponse<IEnumerable<TrackDto>>>;

}
