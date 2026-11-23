using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.EnrollmentFeature.CheckInternEnrollmentExists.Queries;
using LMS___Mini_Version.features.EnrollmentFeature.CreateEnrollment.Commands;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Orchestrators;
using LMS___Mini_Version.features.InternFeature.ApproveIntern.Commands;
using LMS___Mini_Version.features.InternFeature.GetInternById.Queries;
using LMS___Mini_Version.features.PaymentFeature.CreatePayment.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Queries;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Queries;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.handler
{
    public class ApproveAndRegisterInternOrchestratorHandler : IRequestHandler<ApproveAndRegisterInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator _mediator;
        public ApproveAndRegisterInternOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternOrchestrator request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();

            if (request.InternId <= 0)
            {
                errors.Add("InternId", new[] { "Intern ID must be greater than zero." });
            }
            if (!Enum.IsDefined(typeof(PaymentMethod), request.Method))
            {
                errors.Add("PaymentMethod", new[] { "Invalid payment method." });
            }
            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }

            var internResponse = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
            if(!internResponse.Success)
            {
                return RequestResponse<int>.Fail(internResponse.StatusCode, internResponse.Errors);
            }
            if(internResponse.Data == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern not found." } } });
            }
            if(internResponse.Data.Status != "Pending")
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern is not Pending." } } });
            }

            var trackResponse = await _mediator.Send(new GetTrackByIdQuery(internResponse.Data.TrackId),cancellationToken);
            if(!trackResponse.Success)
            {
                return RequestResponse<int>.Fail(trackResponse.StatusCode, trackResponse.Errors);
            }
            if (trackResponse.Data == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]> { { "Track", new string[] { "Track not found." } } });
            }
            if(!trackResponse.Data.IsActive)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Track", new string[] { "Track is not active." } } });
            }

            var enrollmentResponse = await _mediator.Send(new CheckInternEnrollmentExistsQuery(request.InternId,internResponse.Data.TrackId),cancellationToken);
            if (!enrollmentResponse.Success)
            {
                return RequestResponse<int>.Fail(enrollmentResponse.StatusCode, enrollmentResponse.Errors);
            }
            if(enrollmentResponse.Data)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Enrollment", new string[] { "Intern already has an active or pending enrollment for this track." } } });
            }

            var availabilityResponse = await _mediator.Send(new GetTrackAvailabilityQuery(internResponse.Data.TrackId), cancellationToken);
            if (!availabilityResponse.Success)
            {
                return RequestResponse<int>.Fail(availabilityResponse.StatusCode, availabilityResponse.Errors);
            }

            if (availabilityResponse.Data == null || availabilityResponse.Data.AvailableSeats <= 0)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Track", new[] { "No available seats in this track." } } });
            }

            var approveResponse = await _mediator.Send(new ApproveInternCommand(request.InternId),cancellationToken);
            if (!approveResponse.Success)
            {
                return RequestResponse<int>.Fail(approveResponse.StatusCode,approveResponse.Errors);
            }

            var createEnrollmentResponse = await _mediator.Send(new CreateEnrollmentCommand(request.InternId,internResponse.Data.TrackId),cancellationToken);
            if (!createEnrollmentResponse.Success)
            {
                return RequestResponse<int>.Fail(createEnrollmentResponse.StatusCode,createEnrollmentResponse.Errors);
            }
            int enrollmentId = createEnrollmentResponse.Data;
            if (enrollmentId <= 0)
            {
                return RequestResponse<int>.Fail(500,new Dictionary<string, string[]>{{ "Enrollment", new[] { "Failed to create enrollment." } }});
            }

            var createPaymentResponse = await _mediator.Send(new CreatePaymentCommand(enrollmentId,trackResponse.Data.Fees,request.Method),cancellationToken);
            if (!createPaymentResponse.Success)
            {
                return RequestResponse<int>.Fail(createPaymentResponse.StatusCode,createPaymentResponse.Errors);
            }
            if (createPaymentResponse.Data <= 0)
            {
                return RequestResponse<int>.Fail(500,new Dictionary<string, string[]>{{ "Payment", new[] { "Failed to create payment." } }});
            }

            return RequestResponse<int>.OK(enrollmentId);
        }
    }
}
