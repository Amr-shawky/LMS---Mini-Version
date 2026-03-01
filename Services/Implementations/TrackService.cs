using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{
    /// <summary>
    /// [Trap 3 Fix] All methods are fully async — no synchronous ToList() or SaveChanges().
    /// [Trap 4 Fix] Uses GetTable() (IQueryable) to build DB-side queries, then materializes with ToListAsync().
    /// [Trap 5 Fix] Contains only single-entity "Steps" — no cross-entity orchestration.
    /// [Trap 6 Fix] Does NOT call SaveChanges/CompleteAsync — that's the Mediator's or Controller's job.
    /// </summary>
    public class TrackService : ITrackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrackDto>> GetAllAsync()
        {
            // [Trap 4 Fix] Use IQueryable → Include → ToListAsync so everything runs in the DB
            var tracks = await _unitOfWork.Tracks
                .GetTable()
                .Include(t => t.Enrollments)
                .ToListAsync()
                .ConfigureAwait(false);

            return tracks.Select(t => t.ToDto());
        }

        public async Task<TrackDto?> GetByIdAsync(int id)
        {
            // FindAsync leverages the Change Tracker (avoids redundant DB hits)
            var track = await _unitOfWork.Tracks.GetByIdAsync(id).ConfigureAwait(false);
            return track?.ToDto();
        }

        public async Task<TrackDto> CreateAsync(TrackDto dto)
        {
            var entity = new Domain.Entities.Track
            {
                Name = dto.Name,
                Fees = dto.Fees,
                IsActive = dto.IsActive,
                MaxCapacity = dto.MaxCapacity
            };

            _unitOfWork.Tracks.Add(entity);
            // No SaveChanges here — Controller will call _unitOfWork.CompleteAsync()
            return entity.ToDto();
        }

        public async Task<bool> UpdateAsync(int id, TrackDto dto)
        {
            var track = await _unitOfWork.Tracks.GetByIdAsync(id).ConfigureAwait(false);
            if (track == null) return false;

            track.Name = dto.Name;
            track.Fees = dto.Fees;
            track.IsActive = dto.IsActive;
            track.MaxCapacity = dto.MaxCapacity;

            _unitOfWork.Tracks.Update(track);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var track = await _unitOfWork.Tracks.GetByIdAsync(id).ConfigureAwait(false);
            if (track == null) return false;

            _unitOfWork.Tracks.Delete(track);
            return true;
        }

        /// <summary>
        /// Business Step: checks if the track's active enrollment count is below MaxCapacity.
        /// </summary>
        public async Task<bool> CheckCapacityAsync(int trackId)
        {
            var activeCount = await _unitOfWork.Enrollments
                .GetTable()
                .CountAsync(e => e.TrackId == trackId
                              && e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
                .ConfigureAwait(false);

            var track = await _unitOfWork.Tracks.GetByIdAsync(trackId).ConfigureAwait(false);
            if (track == null) return false;

            return activeCount < track.MaxCapacity;
        }
    }
}
