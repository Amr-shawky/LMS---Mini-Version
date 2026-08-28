using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class UpdateEnrollmentTrackCommandHandler
    (IGeneralRepository<Enrollment> _enrollmentrepo) 
    : IRequestHandler<UpdateEnrollmentTrackCommand, Unit>
{


    public async Task<Unit> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentrepo.GetTable()
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

        if (enrollment == null)
            throw new Exception("Enrollment not found");

        enrollment.TrackId = request.NewTrackId;
        
        return Unit.Value;
    }
}