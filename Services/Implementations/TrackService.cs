using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{

    public class TrackService : ITrackService
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TrackService(
            IGeneralRepository<Track> trackRepository,
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _trackRepository = trackRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrackDto>> GetAllAsync()
        {
            var tracks = await _trackRepository
                .GetAll()
                .Include(t => t.Enrollments)
                .ToListAsync()
                .ConfigureAwait(false);

            return tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Name = t.Name,
                Fees = t.Fees,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity,
                CurrentEnrollmentCount = t.Enrollments.Count
            });
        }

        public async Task<TrackDto?> GetByIdAsync(int id)
        {
            var track = await _trackRepository.GetAll()
                .Include(t => t.Enrollments)
                .FirstOrDefaultAsync(t => t.Id == id)
                .ConfigureAwait(false);

            return track is null ? null : new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Fees = track.Fees,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                CurrentEnrollmentCount = track.Enrollments.Count
            };
        }

        public async Task<TrackDto> CreateAsync(TrackDto dto)
        {
            var entity = new Track
            {
                Name = dto.Name,
                Fees = dto.Fees,
                IsActive = dto.IsActive,
                MaxCapacity = dto.MaxCapacity
            };

            _trackRepository.Add(entity);

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return new TrackDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Fees = entity.Fees,
                IsActive = entity.IsActive,
                MaxCapacity = entity.MaxCapacity,
                CurrentEnrollmentCount = entity.Enrollments.Count
            };
        }

        public async Task<bool> UpdateAsync(int id, TrackDto dto)
        {
            var track = await _trackRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (track == null) return false;

            track.Name = dto.Name;
            track.Fees = dto.Fees;
            track.IsActive = dto.IsActive;
            track.MaxCapacity = dto.MaxCapacity;

            _trackRepository.Update(track);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var track = await _trackRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (track == null) return false;

            _trackRepository.Delete(track);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> CheckCapacityAsync(int trackId)
        {
            var activeCount = await _enrollmentRepository
                .GetAll()
                .CountAsync(e => e.TrackId == trackId
                              && e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
                .ConfigureAwait(false);

            var track = await _trackRepository.GetByIdAsync(trackId).ConfigureAwait(false);
            if (track == null) return false;

            return activeCount < track.MaxCapacity;
        }
    }
}
