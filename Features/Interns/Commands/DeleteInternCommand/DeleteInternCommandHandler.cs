using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.DeleteInternCommand
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand,bool>
    {
        private readonly IUnitOfWork _uow;
        public DeleteInternCommandHandler(IUnitOfWork uow) => _uow = uow;
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _uow.Interns.GetByIdAsync(request.InternId).ConfigureAwait(false);
            if(intern == null) throw new InvalidOperationException($"Inter with Id {request.InternId} Doesn't exist");
            _uow.Interns.Delete(intern);
            await _uow.CompleteAsync().ConfigureAwait(false);
            return true;
        }
    }
}