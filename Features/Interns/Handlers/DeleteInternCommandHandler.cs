using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Intern> _internRepository;

        public DeleteInternCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Intern> internRepository)
        {
            _unitOfWork = unitOfWork;
            _internRepository = internRepository;
        }
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetByIdAsync(request.id);
            if (intern == null) return false; 

            _internRepository.Delete(intern);
            await _unitOfWork.CompleteAsync();
            return true; 
        }
    }
}
