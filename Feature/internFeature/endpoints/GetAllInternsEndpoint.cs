using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.internFeature.endpoints
{
    public static class GetAllInternsEndpoint
    {
        public static void MapGetAllInternsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Intern/v2", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAllInternsQuery());
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse<IEnumerable<InternSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data!.Select(d => d.ToSummaryViewModel());
                return Results.Ok(EndpointResponse<IEnumerable<InternSummaryViewModel>>.Ok(viewModels));
            }).WithTags("Intern V2")
            .WithSummary("Get all interns (v2)");
        }
    }
}
