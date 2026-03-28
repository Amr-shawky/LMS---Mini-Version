using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.CQRS.Track.Queries;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingintern = await _mediator.Send(new IsInternEmailExistQuery(request.Email), cancellationToken);
            if (existingintern.Result == true)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            _uow.Interns.Add(new Domain.Entities.Intern { 
                Email = request.Email,
                FullName = request.FullName,
                BirthYear = request.BirthYear,
                Status = request.Status,
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
