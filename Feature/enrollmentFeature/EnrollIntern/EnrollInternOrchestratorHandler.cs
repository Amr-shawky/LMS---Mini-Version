using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.GetEnrollmentsByIntern;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Feature.paymentFeature.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.EnrollIntern
{
    public class EnrollInternOrchestratorHandler : IRequestHandler<EnrollInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(EnrollInternOrchestrator request, CancellationToken cancellationToken)
        {
            var internResponse = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
            if (!internResponse.Success || internResponse.Data == null)
            {
                return RequestResponse<int>.Fail($"Intern with ID {request.InternId} was not found.", 404);
            }

            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse<int>.Fail($"Track with ID {request.TrackId} was not found.", 404);
            }

            var track = trackResponse.Data;
            if (!track.IsActive)
            {
                return RequestResponse<int>.Fail($"Track '{track.Name}' is not currently active.", 400);
            }

            var internEnrollments = await _mediator.Send(new GetEnrollmentsByInternQuery(request.InternId, 1, 100), cancellationToken);
            if (internEnrollments.Success && internEnrollments.Data != null &&
                internEnrollments.Data.Items.Any(e => e.TrackId == request.TrackId && e.Status == EnrollmentStatus.Active))
            {
                return RequestResponse<int>.Fail($"Intern with ID {request.InternId} is already actively enrolled in Track '{track.Name}'.", 400);
            }

            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.TrackId), cancellationToken);
            if (activeCount >= track.MaxCapacity)
            {
                return RequestResponse<int>.Fail($"Track '{track.Name}' has reached its maximum capacity.", 400);
            }

            RequestResponse<int> enrollmentResult = null!;

            await _unitOfWork.ExecuteAsync(async () =>
            {
                enrollmentResult = await _mediator.Send(new CreateEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

                if (track.Fees > 0)
                {
                    await _mediator.Send(new CreatePaymentCommand(
                        enrollmentResult.Data,
                        track.Fees,
                        PaymentMethod.Cash,
                        PaymentStatus.Pending), cancellationToken);
                }
            });

            return enrollmentResult;
        }
    }
}
