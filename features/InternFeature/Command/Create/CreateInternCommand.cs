using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Command.Create;

public record CreateInternCommand(
    string FullName,
    string Email,
    int BirthYear,
    int TrackId) : IRequest<RequestResponse<int>>;