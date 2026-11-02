using LMS___Mini_Version.Features.Intern.AddIntern.Command;
using LMS___Mini_Version.Features.Intern.AddIntern.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LMS___Mini_Version.Features.Intern.AddIntern.EndPoint
{
    public static class AddInternEndPoint
    {
        public static void AddIntern(this WebApplication app)
        {
            app.MapPost("api/Enrollment/v2", async (AddInternDto internDto,IMediator _mediator) =>
            {
              var flag= await _mediator.Send(new AddInternCommand
                    (internDto.FullName, internDto.Email, internDto.BirthYear, internDto.Status, internDto.TrackId));
                if (flag)
                {
                    return Results.Created();
                }

                return Results.BadRequest();

            });
        }
    }
}
