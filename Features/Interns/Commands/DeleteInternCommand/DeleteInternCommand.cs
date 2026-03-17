using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.DeleteInternCommand;

public record DeleteInternCommand(int InternId):IRequest<bool>;