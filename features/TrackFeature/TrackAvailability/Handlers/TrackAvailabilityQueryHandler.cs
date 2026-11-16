using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackAvailability.DTOs;
using LMS___Mini_Version.features.TrackFeature.TrackAvailability.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS___Mini_Version.features.TrackFeature.TrackAvailability.Handlers;

public class TrackAvailabilityQueryHandler(IGeneralRepository<Track> repository) :
    IRequestHandler<TrackAvailabilityQuery, RequestResponse<TrackAvailabilityDTo>>
{
    public async Task<RequestResponse<TrackAvailabilityDTo>> Handle(TrackAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var track = await repository.GetAll().
            Where(x=>x.Id  == request.Id)
            .Select(x=> new TrackAvailabilityDTo
            {
                IsActive = x.IsActive,
                MaxCapacity = x.MaxCapacity,
                TrackName = x.Name,
                ReservedSeats = x.Enrollments.Count(x=>x.Status == EnrollmentStatus.Active
                                                    ||x.Status == EnrollmentStatus.Pending)
            }).FirstOrDefaultAsync(cancellationToken);

        if(track == null)
        {
            return RequestResponse<TrackAvailabilityDTo>.Fail(404, new Dictionary<string, string[]>
            {
                ["track"] = ["track not found"]
            });
        }

        var AvailableSeats = Math.Max(0, (track.MaxCapacity - track.ReservedSeats));

        var dto = new TrackAvailabilityDTo { 
        
            AvailableSeats = AvailableSeats,
            ReservedSeats= track.ReservedSeats,
            IsActive = track.IsActive,
            MaxCapacity = track.MaxCapacity,
            TrackName = track.TrackName 
           
        };

       return RequestResponse<TrackAvailabilityDTo>.OK(dto,200);

        
    }
}
