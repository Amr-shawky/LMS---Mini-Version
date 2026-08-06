using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class TransferEnrollmentCommandHandler(
    IEnrollmentService enrollmentService,
    ITrackService trackService,
    IUnitOfWork unitOfWork) : IRequestHandler<TransferEnrollmentCommand, bool>
{
    public async Task<bool> Handle(
     TransferEnrollmentCommand request,
     CancellationToken cancellationToken)
    {
        var enrollment = await enrollmentService.GetByIdAsync(request.EnrollmentId);

        if (enrollment == null)
            throw new Exception("Enrollment not found");

        if (enrollment.Status == EnrollmentStatus.Cancelled)
            throw new Exception("Enrollment is cancelled");

        if (enrollment.TrackId == request.NewTrackId)
            throw new Exception("Same Track");

        var track = await trackService.GetByIdAsync(request.NewTrackId);

        if (track == null)
            throw new Exception("Track not found");

        if (!track.IsActive)
            throw new Exception("Track not active");

        var hasCapacity = await trackService.CheckCapacityAsync(request.NewTrackId);

        if (!hasCapacity)
            throw new Exception("Track is full");

        var updated = await enrollmentService.UpdateTrackAsync(
            request.EnrollmentId,
            request.NewTrackId);

        if (!updated)
            return false;

        await unitOfWork.CompleteAsync();

        return true;
    }
}
