using Microsoft.AspNetCore.Mvc;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;
using LMS___Mini_Version.Features.Shared.LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Features.Tracks.DTOs;
using LMS___Mini_Version.Features.Tracks.VMs;
using MediatR;
using LMS___Mini_Version.Features.Tracks.Queries;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //public IEnumerable<Track> GetAll()
        //{
        //    var tracks = _context.Tracks.ToList();
        //    return tracks;
        //}

        [HttpGet("{id}/availability")]
        public async Task<EndpointResponse<CheckTrackAvailabilityViewModel>> GetDetailsById(int id,CancellationToken cancellationToken)
        {
            var query = await _mediator.Send(new CheckTrackAvailabilityQuery(id),cancellationToken);
            if (!query.Success)
                return EndpointResponse<CheckTrackAvailabilityViewModel>.Fail(404, "Track is not found");

            var viewmodel = new CheckTrackAvailabilityViewModel
            {
                TrackName = query.Data.TrackName,
                IsActive = query.Data.IsActive,
                MaxCapacity = query.Data.MaxCapacity,
                ReservedSeats = query.Data.ReservedSeats,
                AvailableSeats = query.Data.AvailableSeats
            };

            var result = EndpointResponse<CheckTrackAvailabilityViewModel>.OK(viewmodel);
            return result;
        }

        //[HttpPost]
        //public ActionResult Create(Track track)
        //{
        //    _context.Tracks.Add(track);
        //    _context.SaveChanges();

        //    return Ok(track);
        //}

        //[HttpPut("{id}")]
        //public ActionResult Update(int id, Track updatedTrack)
        //{
        //    var track = _context.Tracks.Find(id);
        //    if (track == null) return NotFound();

        //    track.Name = updatedTrack.Name;
        //    track.Fees = updatedTrack.Fees;
        //    track.IsActive = updatedTrack.IsActive;

        //    _context.SaveChanges();
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    var track = _context.Tracks.Find(id);
        //    if (track == null) return NotFound();

        //    _context.Tracks.Remove(track);
        //    _context.SaveChanges();

        //    return NoContent();
        //}
    }
}