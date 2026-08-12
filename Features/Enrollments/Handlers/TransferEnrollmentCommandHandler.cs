using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public TransferEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo,
                                                IUnitOfWork unitOfWork,
                                                IMediator mediator)
        {
            _enrollmentRepo = enrollmentRepo;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {
            //get enrollment 
            var enrollment =await _enrollmentRepo.GetByIdAsync(request.id);
            if (enrollment is null|| enrollment.TrackId == request.newTrackId)
                return Unit.Value;
            //is track exist 
            
            var newTrack = await _mediator.Send(new GetTrackByIdQuery() { Id=request.newTrackId}, cancellationToken);
            if (newTrack is null)
                return Unit.Value;
            //check new track capacity
            if(newTrack.CurrentEnrollmentCount>=newTrack.MaxCapacity)
                return Unit.Value;
            //transfer
            enrollment.TrackId= request.newTrackId;
            //payment fees
            await _mediator.Send(new UpdatePaymentAmountByEnrollmentIdCommand(request.id, newTrack.Fees),cancellationToken);
            //save
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
