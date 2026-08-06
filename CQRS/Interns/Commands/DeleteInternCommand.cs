using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands;

public record DeleteInternCommand(int id): IRequest<bool>;

