using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Commands
{
    public record AcceptInternCommand(int InternId): IRequest<RequestResponse>
    {
    }

    public class AcceptInternCommandHandler : IRequestHandler<AcceptInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AcceptInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse> Handle(AcceptInternCommand request, CancellationToken cancellationToken)
        {
            var exists = await _mediator.Send(new CheckInternExistsQuery(request.InternId), cancellationToken);

            if(!exists.Success || exists.Data == false)
                return RequestResponse.Fail(ErrorCode.InternNotFound);

            var intern = new Intern { Id = request.InternId, Status = "Accepted" }; 

            _repository.SaveIncludes(intern, nameof(Intern.Status));

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
