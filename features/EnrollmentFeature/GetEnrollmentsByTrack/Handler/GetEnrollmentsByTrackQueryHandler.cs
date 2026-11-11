using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Handler
{
    public class GetEnrollmentsByTrackQueryHandler
        : IRequestHandler<GetEnrollmentsByTrackQuery,
            RequestResponse<IEnumerable<EnrollmentByTrackDto>>>
    {
        private readonly IGeneralRepository<Enrollment> repository;
        private readonly ISender sender;

        public GetEnrollmentsByTrackQueryHandler(
            IGeneralRepository<Enrollment> repository,
            ISender sender)
        {
            this.repository = repository;
            this.sender = sender;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentByTrackDto>>> Handle(
            GetEnrollmentsByTrackQuery request,
            CancellationToken cancellationToken)
        {
            var trackResult = await sender.Send(
                new GetTrackByIdQuery(request.TrackId),
                cancellationToken);

            if (!trackResult.Success || trackResult.Data is null)
            {
                return RequestResponse<IEnumerable<EnrollmentByTrackDto>>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track not found." }
                    });
            }
            var enrollments = await repository.GetAll()
                .Where(e => e.TrackId == request.TrackId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Select(e => new EnrollmentByTrackDto
                {
                    EnrollmentId = e.Id,
                    InternName = e.Intern.FullName,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.Status,

                    PaymentAmount = e.Payment != null
                        ? e.Payment.Amount
                        : null,

                    PaymentStatus = e.Payment != null
                        ? (PaymentStatus?)e.Payment.Status
                        : null
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<EnrollmentByTrackDto>>.OK(
                enrollments);
        }
    }
}