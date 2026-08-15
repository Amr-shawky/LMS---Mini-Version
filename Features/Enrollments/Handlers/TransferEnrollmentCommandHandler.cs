using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand>
{
    private readonly ITrackService _trackService;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;

    public TransferEnrollmentCommandHandler(ITrackService trackService, IEnrollmentService enrollmentService, IPaymentService paymentService, IUnitOfWork unitOfWork)
    {
        _trackService = trackService;
        _enrollmentService = enrollmentService;
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
    {
        // Check if the new track has capacity
        var hasCapacity = await _trackService.CheckCapacityAsync(request.NewTrackId);
        if (!hasCapacity) return;

        await _enrollmentService.UpdateTrackAsync(request.EnrollmentId, request.NewTrackId);


        // Retrieve the new track details to get the fees
        var newTrack = await _trackService.GetByIdAsync(request.NewTrackId);
        // Update the payment amount based on the new track's fees
        if (newTrack != null)
        {
            
            await _paymentService.UpdatePaymentAmountAsync(request.EnrollmentId, newTrack.Fees);
        }
        await _unitOfWork.CompleteAsync();
    }
}
