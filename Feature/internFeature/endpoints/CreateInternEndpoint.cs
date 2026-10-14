using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Commands;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.internFeature.endpoints
{
    public static class CreateInternEndpoint
    {
        public static void MapCreateInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Intern/v2", async (CreateInternViewModel vm, IMediator mediator) =>
            {
                var createResponse = await mediator.Send(new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId));
                if (!createResponse.Success)
                {
                    return Results.BadRequest(EndpointResponse<InternSummaryViewModel>.Fail(createResponse.Message, createResponse.StatusCode, createResponse.Errors));
                }

                var internResponse = await mediator.Send(new GetInternByIdQuery(createResponse.Data));
                var summaryVm = internResponse.Data != null
                    ? internResponse.Data.ToSummaryViewModel()
                    : new InternSummaryViewModel { Id = createResponse.Data, FullName = vm.FullName, Email = vm.Email, Status = vm.Status };

                return Results.Created($"/api/Intern/v2/{createResponse.Data}", EndpointResponse<InternSummaryViewModel>.Created(summaryVm, "Intern created successfully"));
            }).WithTags("Intern V2")
            .WithSummary("Create intern (v2)");
        }
    }
}
