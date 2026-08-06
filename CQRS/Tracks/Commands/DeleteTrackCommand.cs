using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands;

public record DeleteTrackCommand(int id) : IRequest<bool>;

