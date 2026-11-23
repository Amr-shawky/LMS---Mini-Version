using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.ApproveIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveIntern.handler
{
    public class ApproveInternCommandHandler : IRequestHandler<ApproveInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ApproveInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(ApproveInternCommand request, CancellationToken cancellationToken)
        {
           if(request.InternId <= 0)
            {
                return RequestResponse.Fail(400, new Dictionary<string, string[]> { { "InternId", new[] { "Intern ID must be greater than zero." } } });
            }
            var intern = await _repository.GetByIdAsync(request.InternId);
            if (intern is null)
            {
                return RequestResponse.Fail(404, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern not found." } } });
            }
            if (intern.Status != "Pending")
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern is not Pending." } } });
            }
            intern.Status = "Accepted";
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();
        }
    }
}
