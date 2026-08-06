namespace LMS___Mini.CQRS.Interns.Commands.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, bool>
    {

        private readonly IUnitOfWork _unitOf;
        public DeleteInternCommandHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;   
        }
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _unitOf.Interns.GetByIdAsync(request.InternId);
            if (intern == null) 
                throw new InvalidOperationException($"Inter with Id {request.InternId} Doesn't exist");

            _unitOf.Interns.Delete(intern);
            await _unitOf.CompleteAsync();   // Hard Delete

            return true;
        }

    }
}
