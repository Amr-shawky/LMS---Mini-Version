using LMS___Mini_Version.Domain.Constants;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Command.CreateEnrollmentWithPayment;
using LMS___Mini_Version.features.EnrollmentFeature.Queries.HasActiveOrPendingEnrollment;
using LMS___Mini_Version.features.EnrollmentFeature.Queries.HasAvailableSeats;
using LMS___Mini_Version.features.InternFeature.Queries.GetById;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.Orchestrators;

public class ApproveAndRegisterInternOrchestratorCommandHandler(
    IMediator mediator,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveAndRegisterInternOrchestratorCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(
        ApproveAndRegisterInternOrchestratorCommand request, CancellationToken ct)
    {
        if (request.PaymentMethod is null || !Enum.IsDefined(request.PaymentMethod.Value))
            return Fail(400, "PaymentMethod",
                "PaymentMethod is required and must be one of: Cash, CreditCard, BankTransfer, Online");

        var method = request.PaymentMethod.Value;

        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var intern = await mediator.Send(new GetInternByIdQuery(request.InternId), ct);
            if (intern is null)
                return Fail(404, "InternId", "Intern not found");

            if (intern.Status != InternStatus.Pending)
                return Fail(409, "Status",
                    $"Only a Pending intern can be approved (current status: {intern.Status})");

            var track = await mediator.Send(new GetTrackByIdQuery(intern.TrackId), ct);
            if (track is null)
                return Fail(404, "TrackId", "Track not found");

            if (!track.IsActive)
                return Fail(409, "TrackId", "Track is not active");

            if (await mediator.Send(new HasActiveOrPendingEnrollmentQuery(intern.Id, track.Id), ct))
                return Fail(409, "Enrollment",
                    "Intern already has a Pending or Active enrollment in this track");

            if (!await mediator.Send(new HasAvailableSeatsQuery(track.Id, track.MaxCapacity), ct))
                return Fail(409, "Track", "No available seats in this track");

            intern.Status = InternStatus.Accepted;

            var enrollment = await mediator.Send(
                new CreateEnrollmentWithPaymentCommand(intern.Id, track.Id, track.Fees, method), ct);

            await unitOfWork.SaveChangesAsync();

            return RequestResponse<int>.OK(enrollment.Id, 201);
        },
        ct);
    }

    private static RequestResponse<int> Fail(int code, string key, string message)
        => RequestResponse<int>.Fail(code, new Dictionary<string, string[]> { { key, [message] } });
}