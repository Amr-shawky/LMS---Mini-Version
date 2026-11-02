using LMS___Mini_Version.Features.Enrollment.GetAllEnrollment.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.GetAllEnrollment.EndPoints
{
    public static class GetAllEnrollmentsEndpoint
    {
        public static  void GetAllEnrollments(this WebApplication app)
        {
            app.MapGet("api/Enrollment/v2",async (int pageIndex,int pageSize,IMediator _mediator)=>{
              var enrollments= await _mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));

                
                return Results.Ok(enrollments);
            }).WithTags("Enrollments");
        }
    }
}
