using LMS___Mini_Version.Features.Intern.GetInternById.Query;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.GetInternById.EndPoint
{
    public static class GetInternByIdEndpoint
    {
        public static void GetInternById(this WebApplication app)
        {
            app.MapGet("api/Intern/{id}/v2", async (int internId, IMediator _mediator) =>
            {
               var track = await _mediator.Send(new GetInternByIdQuery(internId));
                if(track == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(track);
            }).WithTags("Interns");
        }
    }
}
