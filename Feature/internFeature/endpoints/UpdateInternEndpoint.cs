using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Commands;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.internFeature.endpoints
{
    public static class UpdateInternEndpoint
    {
        public static void MapUpdateInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/Intern/v2/{id:int}", async (int id, UpdateInternViewModel vm, IMediator mediator) =>
            {
                var response = await mediator.Send(new UpdateInternCommand(id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId));
                if (!response.Success)
                {
                    return Results.NotFound(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Intern updated successfully"));
            }).WithTags("Intern V2")
            .WithSummary("Update intern (v2)");
        }
    }
}
