using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class UpdateStatusEnrollmentCommandHandler : IRequestHandler<UpdateStatusEnrollmentCommand, RequestResult<bool>>
    {
        //private readonly IMediator mediator;
        private readonly IGeneralRepository<Enrollment> _repository;
        public UpdateStatusEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResult<bool>> Handle(UpdateStatusEnrollmentCommand request, CancellationToken cancellationToken)
        {

            var enroll = await _repository.GetById(request.enrollmentId);
            if (enroll == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.EnrollMentNotExist);
            }
            enroll.Status = request.newStatus;

            _repository.Update(enroll);

            return await Task.FromResult(RequestResult<bool>.Success(true));
        }
    }

}
