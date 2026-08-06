 using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler: IRequestHandler<TransferEnrollmentCommand>
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;
        private readonly ITrackService _trackService;


        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentCommandHandler(IEnrollmentService enrollmentService, IPaymentService paymentService, IUnitOfWork unitOfWork, ITrackService trackService)
        {
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _trackService = trackService;
        }

        public async Task<Unit> Handle( TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {

            var enrollment = await _enrollmentService.GetByIdAsync(request.EnrollmentId);
            if (enrollment == null)
                throw new Exception("enrollment not found");

            if (enrollment.Status == EnrollmentStatus.Cancelled)
                throw new Exception("Can`t transfer  cancelled enrollment");
            
            if(request.NewTrackId == enrollment.TrackId)
                throw new Exception("student already in same track");


            var newTrack = await _trackService.GetByIdAsync(request.NewTrackId);
            if (newTrack == null)
            {
                *
            }
                throw new Exception("Track  not found");#if


            if (!newTrack.IsActive)
                throw new Exception("Track  not active");


            if (await _trackService.CheckCapacityAsync(request.NewTrackId))
                throw new Exception("Track is full capacity");


            await _enrollmentService.UpdateTrackAsync(request.EnrollmentId,request.NewTrackId);

            await _paymentService.UpdatePaymentAmountAsync(request.EnrollmentId, newTrack.Fees);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
