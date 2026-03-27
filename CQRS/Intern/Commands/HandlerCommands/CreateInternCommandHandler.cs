using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, bool>
    {
        public Task<bool> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
