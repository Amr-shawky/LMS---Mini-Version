using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class updateEnrollmentStatusCommandHandler : IRequestHandler<updateEnrollmentStatusCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        
        public updateEnrollmentStatusCommandHandler(IUnitOfWork unitOfWork, IMediator mediator,IGeneralRepository<Enrollment> enrollmentrepo) 
        {
            _enrollmentrepo = enrollmentrepo;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(updateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {

            Enrollment enrollmententity =await _enrollmentrepo.GetByIdAsync(request.enrollmentId);

            if (enrollmententity == null)
            {
                throw new KeyNotFoundException($"Enrollment {request.enrollmentId} not found.");
            }
            
            enrollmententity.Status = request.Status;
            _enrollmentrepo.Update(enrollmententity);
            await _unitOfWork.CompleteAsync();
            
            return Unit.Value;
        }
    }
}

            //var enrollmententity = _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentId));
            //Enrollment updatedEnrollmentEntity = new Enrollment
            //{
            //    Id = request.enrollmentId,
            //    Status = request.Status
            //};
            //if (updatedEnrollmentEntity != null)
            //{
            //    _enrollmentrepo.Update(updatedEnrollmentEntity);
            //    await _unitOfWork.CompleteAsync();
            //}
            //  return Unit.Value;