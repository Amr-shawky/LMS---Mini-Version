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
            return await _trackRepository
                .GetAll()
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive,
                    MaxCapacity = t.MaxCapacity,
                    CurrentEnrollmentCount = t.Enrollments.Count
                })
                .ToListAsync();
        }

        public async Task<TrackDto?> GetByIdAsync(int id)
        {
            return await _trackRepository
                .GetAll()
                .Where(t => t.Id == id)
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive,
                    MaxCapacity = t.MaxCapacity,
                    CurrentEnrollmentCount = t.Enrollments.Count
                })
                .FirstOrDefaultAsync();
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

            await _unitOfWork.SaveChangesAsync();

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
            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null) return false;

            track.Name = dto.Name;
            track.Fees = dto.Fees;
            track.IsActive = dto.IsActive;
            track.MaxCapacity = dto.MaxCapacity;

            _trackRepository.Update(track);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null) return false;

            _trackRepository.Delete(track);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckCapacityAsync(int trackId)
        {
            var activeCount = await _enrollmentRepository
                .GetAll()
                .CountAsync(e => e.TrackId == trackId
                              && e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
                ;

            var track = await _trackRepository.GetByIdAsync(trackId);
            if (track == null) return false;

            return activeCount < track.MaxCapacity;
        }
    }
}
