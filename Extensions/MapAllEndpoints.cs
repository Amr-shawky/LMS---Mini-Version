using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.enrollmentFeature.endpoints;
using LMS___Mini_Version.Feature.internFeature.endpoints;
using LMS___Mini_Version.Feature.paymentFeature.endpoints;
using LMS___Mini_Version.Feature.Tracks.endpoints;
using LMS___Mini_Version.Feature.Tracks.Query;
using MassTransit;
using MassTransit.Mediator;

namespace LMS___Mini_Version.Extensions
{
    public static class MapAllEndpoints
    {
        public static void mapAllEndpoints(this IEndpointRouteBuilder app)
        {
            // ─── Track Minimal API v2 Endpoints ────────────────────────
            app.MapGetAllTracksEndpoint();
            app.MapGetTrackByIdEndpoint();
            app.MapCreateTrackEndpoint();
            app.MapUpdateTrackV2Endpoint();
            app.MapDeleteTrackEndpoint();

            // ─── Intern Minimal API v2 Endpoints ───────────────────────
            app.MapGetAllInternsEndpoint();
            app.MapGetInternByIdEndpoint();
            app.MapCreateInternEndpoint();
            app.MapUpdateInternEndpoint();
            app.MapDeleteInternEndpoint();

            // ─── Payment Minimal API v2 Endpoints ──────────────────────
            app.MapGetAllPaymentsEndpoint();
            app.MapGetPaymentByEnrollmentEndpoint();

            // ─── Enrollment Minimal API v2 Endpoints ───────────────────
            app.MapGetAllEnrollmentsEndpoint();
            app.MapGetEnrollmentByIdEndpoint();
            app.MapGetEnrollmentsByInternEndpoint();
            app.MapEnrollInternEndpoint();
            app.MapCancelEnrollmentEndpoint();
            app.MapTransferEnrollmentEndpoint();
            app.MapUpdateTrackEndpoint();

            app.MapPost("api/test/visa", async (IPublishEndpoint publish) => {

                await publish.Publish(new PaymentProcessedMessage { PaymentId = 2, Amount = 40 }

                ,
                context => {

                    context.SetRoutingKey("visa");
                });

                return Results.Ok("visa payment event published");

            });

            app.MapPost("api/test/cash", async (IPublishEndpoint publish) => {

                await publish.Publish(new PaymentProcessedMessage { PaymentId = 2, Amount = 40 }
                ,
                context => {

                    context.SetRoutingKey("cash");
                });

                return Results.Ok("cash payment event published");

            });

        }
    }
}
