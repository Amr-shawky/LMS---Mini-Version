using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class ApproveInternCommandHandler : IRequestHandler<ApproveInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> internRepo;
        private readonly IUnitOfWork unitOfWork;

        public ApproveInternCommandHandler(IGeneralRepository<Intern> internRepo , IUnitOfWork unitOfWork)
        {
            this.internRepo = internRepo;
            this.unitOfWork = unitOfWork;
        }
        public async  Task<RequestResponse> Handle(ApproveInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await internRepo.GetByIdAsync(request.internId);
            if(intern==null)
            {
                return RequestResponse.Fail(400);
            }
            if(intern.Status!="Pending")
            {
                return RequestResponse.Fail(400);
            }
            intern.Status = "Accepted";
            internRepo.Update(intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK(200);

        }
    }
}
