using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> internRepo;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public DeleteInternCommandHandler(IGeneralRepository<Intern> internRepo, IMediator mediator ,IUnitOfWork unitOfWork )
        {
            this.internRepo = internRepo;
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await internRepo.GetByIdAsync(request.internId);
            if(intern==null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
        {
            { "Intern", new[] { "Cannot find intern with that ID." } }
        };

                return RequestResponse.Fail(404, errors);
            }


            var response = await mediator.Send(new GetEnrollementWithInternIdQuery(request.internId));

            if(!response.Success)
            {
                return RequestResponse.Fail(response.StatusCode, response.Errors);
            }

            if(response.Data !=null && response.Data.Any())
             {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Enrollement" , new[]{" Cannot Delete Intern with Enrollements"} }
                };
                return RequestResponse.Fail(400, errors);
            }
           
            
            internRepo.Delete(intern);
           var res = await  unitOfWork.SaveChangesAsync();
            return res > 0 ? RequestResponse.OK(200) : RequestResponse.Fail(400);

        }
    }
}
