using LMS___Mini_Version.CQRS.Enrollment.Queries;
using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands.HandlerCommands
{
    public class UpdateEnrollmentCommandHandler : IRequestHandler<UpdateEnrollmentCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingintern = await _mediator.Send(new GetEnrollmentbyIdQuery(request.id), cancellationToken);
            if (existingintern == null)
                return RequestResult<bool>.Failure(ErrorCode.NotExist);
            _uow.Enrollments.Update(new Domain.Entities.Enrollment
            {
                Id = request.id,
                InternId = request.internId?? existingintern.Result.InternId,
                TrackId = request.trackId??existingintern.Result.TrackId ,
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
