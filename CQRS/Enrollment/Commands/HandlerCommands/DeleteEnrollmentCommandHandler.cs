using LMS___Mini_Version.CQRS.Enrollment.Queries;
using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands.HandlerCommands
{
    public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingintern = await _mediator.Send(new GetEnrollmentbyIdQuery(request.id), cancellationToken);
            if (existingintern == null)
                return RequestResult<bool>.Failure(ErrorCode.NotExist);
            _uow.Enrollments.Delete(new Domain.Entities.Enrollment
            {
                Id = request.id,
                InternId = existingintern.Result.InternId,
                TrackId = existingintern.Result.TrackId,
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
