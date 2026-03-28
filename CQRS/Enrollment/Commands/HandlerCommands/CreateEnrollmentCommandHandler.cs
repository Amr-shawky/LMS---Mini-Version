using LMS___Mini_Version.CQRS.Enrollment.Queries;
using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands.HandlerCommands
{
    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        public async Task<RequestResult<bool>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            var existingintern = await _mediator.Send(new IsEnrollmentExistQuery(request.internId,request.trackId), cancellationToken);
            if (existingintern.Result == true)
                return RequestResult<bool>.Failure(ErrorCode.IsAreadyExist);
            _uow.Enrollments.Add(new Domain.Entities.Enrollment
            {
                InternId = request.internId,
                TrackId = request.trackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            });
            return await _uow.CompleteAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.None);
        }
    }
}
