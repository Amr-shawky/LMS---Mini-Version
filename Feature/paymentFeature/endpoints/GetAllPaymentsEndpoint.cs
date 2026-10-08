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
            app.MapGet("api/Payment/v2", async (IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var response = await mediator.Send(new GetAllPaymentsQuery(pageIndex, pageSize));
                if (!response.Success || response.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<PaginatedResult<PaymentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data.Items.Select(d => d.ToViewModel()).ToList();
                var paginated = PaginatedResult<PaymentViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
                return Results.Ok(EndpointResponse<PaginatedResult<PaymentViewModel>>.Ok(paginated));
            }).WithTags("Payment V2")
            .WithSummary("Get all payments paginated (v2)");
        }
    }
}
