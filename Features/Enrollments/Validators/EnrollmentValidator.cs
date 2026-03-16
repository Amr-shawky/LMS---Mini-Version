using System;
using LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;
using LMS___Mini_Version.Infrastructure.Repositories;

namespace LMS___Mini_Version.Features.Enrollments.Validators;

public class EnrollmentValidator
{
    private readonly UnitOfWork _uow;
    public EnrollmentValidator(UnitOfWork uow) => _uow = uow;
    
    public async Task<string?> ValidateAsync(EnrollmentInternCommand command)
    {
        //  check if the intern and track exists
        var intern = await _uow.Interns.GetByIdAsync(command.InternId).ConfigureAwait(false);
        if(intern == null) return $"Intern with Id {command.InternId} doesn't exist";
        var track = await _uow.Tracks.GetByIdAsync(command.TrackId).ConfigureAwait(false);
        if(track == null) return "Track doesn't exist";
        // Check if the track has capacity
        var hasCapacity = await _uow.Tracks.CheckCapacityAsync(command.TrackId).ConfigureAwait(false);
        if(!hasCapacity) return "Track has no capacity";
        // Check if the intern has an active enrollment
        var isDuplicate = await _uow.Enrollments.HasActiveEnrollmentAsync(command.InternId,command.TrackId).ConfigureAwait(false);
        if(isDuplicate) return $"This intern with Id {command.InternId} already has an active enrollment";
        // Check if the track is active
        var isActive = await _uow.Tracks.IsTrackActiveAsync(command.TrackId).ConfigureAwait(false);
        if(!isActive) return "This track is not active";
        return null;
    }
}