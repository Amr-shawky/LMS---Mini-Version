using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Intern> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _repository.GetByIdAsync(request.Id);
            _repository.Delete(intern!);

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();


        }
    }
}
