using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.paymentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.paymentFeature.endpoints
{
    public static class GetPaymentByEnrollmentEndpoint
    {
        public static void MapGetPaymentByEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Payment/v2/enrollment/{enrollmentId:int}", async (int enrollmentId, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetPaymentByEnrollmentQuery(enrollmentId));
                if (!response.Success || response.Data == null)
                {
                    return Results.NotFound(EndpointResponse<PaymentViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse<PaymentViewModel>.Ok(response.Data.ToViewModel()));
            }).WithTags("Payment V2")
            .WithSummary("Get payment by enrollment ID (v2)");
        }
    }
}
