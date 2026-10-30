using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Messaging;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class updateEnrollmentTrackCommandHandler(
    IGeneralRepository<Enrollment> repository, IPublishEndpoint publisher)
    : IRequestHandler<updateEnrollmentTrackCommand, RequestResponse<bool>>
{
    public async Task<RequestResponse<bool>> Handle(updateEnrollmentTrackCommand request, CancellationToken ct)
    {
        var old = await repository.GetAll().Where(e => e.Id == request.EnrollmentID)
            .Select(e => new { e.TrackId }).SingleOrDefaultAsync(ct);
        if (old == null)
            return RequestResponse<bool>.Fail("Enrollment not found", 404);
        if (old.TrackId == request.TrackID)
            return RequestResponse<bool>.Fail("Already enrolled in the destination track", 400);

        // Selective one-step EF UPDATE (no full entity roundtrip).
        repository.SaveInclude(new Enrollment { Id = request.EnrollmentID, TrackId = request.TrackID },
            nameof(Enrollment.TrackId));
        await repository.SaveChangesAsync();
        // With EF Bus Outbox, this Publish is stored in the SAME SQL transaction.
        await publisher.Publish(new EnrollmentTransferred(
            Guid.NewGuid(), request.EnrollmentID, old.TrackId, request.TrackID, DateTime.UtcNow), ct);
        return RequestResponse<bool>.Ok(true);
    }
}
