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
            app.MapGet("api/Intern/v2", async (IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var response = await mediator.Send(new GetAllInternsQuery(pageIndex, pageSize));
                if (!response.Success || response.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<PaginatedResult<InternSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data.Items.Select(d => d.ToSummaryViewModel()).ToList();
                var paginated = PaginatedResult<InternSummaryViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
                return Results.Ok(EndpointResponse<PaginatedResult<InternSummaryViewModel>>.Ok(paginated));
            }).WithTags("Intern V2")
            .WithSummary("Get all interns paginated (v2)");
        }
    }
}
