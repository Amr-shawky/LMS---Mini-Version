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
    public static class GetAllPaymentsEndpoint
    {
        public static void MapGetAllPaymentsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Payment/v2", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAllPaymentsQuery());
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse<IEnumerable<PaymentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data!.Select(d => d.ToViewModel());
                return Results.Ok(EndpointResponse<IEnumerable<PaymentViewModel>>.Ok(viewModels));
            }).WithTags("Payment V2")
            .WithSummary("Get all payments (v2)");
        }
    }
}
