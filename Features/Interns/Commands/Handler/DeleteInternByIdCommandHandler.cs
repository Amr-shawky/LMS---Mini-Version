using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Errors.Exeptions;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.Handler
{
    public class DeleteInternByIdCommandHandler : IRequestHandler<DeleteInternByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteInternByIdCommand request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(request.id);

            if (intern == null)
                throw new NotFoundException($"Entern" , request.id);


            _unitOfWork.Interns.Delete(intern);

            await _unitOfWork.CompleteAsync();

           
            return true;


        }
    }
}
