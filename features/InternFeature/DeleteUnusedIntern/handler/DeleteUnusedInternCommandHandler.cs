using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DeleteUnusedIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.DeleteUnusedIntern.handler
{
    public class DeleteUnusedInternCommandHandler : IRequestHandler<DeleteUnusedInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUnusedInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(DeleteUnusedInternCommand request, CancellationToken cancellationToken)
        {
            if (request.InternId <= 0)
            {
                return RequestResponse.Fail(400, new Dictionary<string, string[]> { { "InternId", new[] { "Intern ID must be greater than zero" } } });
            }
            var intern = await _repository.GetByIdAsync(request.InternId);
            if(intern == null)
            {
                return RequestResponse.Fail(404, new Dictionary<string, string[]> { { "Intern", new[] { "Intern not found" } } });
            }
            var hasEnrollments = await _repository.GetAll()
                .Where(e => e.Id == request.InternId)
                .AnyAsync(e => e.Enrollments.Any(), cancellationToken);
            if(hasEnrollments)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]> { { "Intern", new[] { "Intern has enrollments, cannot be deleted" } } });
            }
            _repository.Delete(intern);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse.OK(200);
        }
    }
}
