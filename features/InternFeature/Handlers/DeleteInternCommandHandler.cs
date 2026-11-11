using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentsFeature.Queries;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteInternCommandHandler(IGeneralRepository<Intern> repository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _repository.GetByIdAsync(request.Id);
            if (intern is null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
        {
            { "Intern", new[] { "Cannot find This intern " } }
        };

                return RequestResponse.Fail(404, errors);
            }
            var Enrollement = await _mediator.Send(new GetInternEnrollmentHistoryQuery(request.Id));
            if(!Enrollement.Success)
                return RequestResponse.Fail(Enrollement.StatusCode, Enrollement.Errors);

            if (Enrollement.Data.Any())
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Enrollement" , new[]{" Cannot Delete Intern with Enrollements"} }
                };
                return RequestResponse.Fail(400, errors);
            }
            _repository.Delete(intern);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? RequestResponse.OK(200) : RequestResponse.Fail(400);

        }
    }
}
