using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Command.DeleteIntern;

public record DeleteInternCommand(int InternId) : IRequest<RequestResponse>;
