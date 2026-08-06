using LMS___Mini_Version.DTOs;
using MediatR;
using System.Diagnostics;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    public record GetTrackByIdQuery(int id) : IRequest<TrackDto?>;



   
}
