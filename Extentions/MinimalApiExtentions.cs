using LMS___Mini_Version.Features.Enrollment.AddEnrollment.Endpoint;
using LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Endpoint;
using LMS___Mini_Version.Features.Enrollment.GetAllEnrollment.EndPoints;
using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Endpoints;
using LMS___Mini_Version.Features.Enrollment.TransferEnrollment.Endpoint;
using LMS___Mini_Version.Features.Intern.AddIntern.EndPoint;
using LMS___Mini_Version.Features.Intern.GetInternById.EndPoint;
using LMS___Mini_Version.Features.Track.GetTrackById.EndPoint;

namespace LMS___Mini_Version.Extentions
{
    public static class MinimalApiExtentions
    {
        public static void AddMinimalApi(this WebApplication app)
        {
            app.GetEnrollmentById();
            app.GetAllEnrollments();
            app.AddIntern();
            app.GetTrackById();
            app.GetInternById();
            app.AddEnrollment();
            app.CancelEnrollment();
            app.TransferEnrollment();
        }
    }
}
