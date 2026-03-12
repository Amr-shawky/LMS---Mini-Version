using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries.GetAllByTrackIdQuery;

public record GetAllByTrackIdQuery(int trackId) : IRequest<IEnumerable<InternDto>>;