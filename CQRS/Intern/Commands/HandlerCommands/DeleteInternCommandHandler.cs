using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingintern = await _mediator.Send(new GetInternbyIdQuery(request.Id), cancellationToken);
            if (existingintern == null)
                return RequestResult<bool>.Failure(ErrorCode.NotExist);
            _uow.Interns.Add(new Domain.Entities.Intern
            {
                Id = request.Id,
                Email = existingintern.Result.Email,
                FullName = existingintern.Result.FullName,
                BirthYear = existingintern.Result.BirthYear,
                Status = existingintern.Result.Status,
                TrackId = existingintern.Result.TrackId
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
