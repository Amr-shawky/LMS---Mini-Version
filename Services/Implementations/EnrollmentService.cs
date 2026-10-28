using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{

    public class EnrollmentService : IEnrollmentService
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        {
            return await _enrollmentRepository
                .GetAll()
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    InternName = e.Intern != null ? e.Intern.FullName : string.Empty,
                    TrackId = e.TrackId,
                    TrackName = e.Track != null ? e.Track.Name : string.Empty,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                })
                .ToListAsync();
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            return await _enrollmentRepository
                .GetAll()
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    InternName = e.Intern != null ? e.Intern.FullName : string.Empty,
                    TrackId = e.TrackId,
                    TrackName = e.Track != null ? e.Track.Name : string.Empty,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentDto dto)
        {
            var entity = new Enrollment
            {
                InternId = dto.InternId,
                TrackId = dto.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending
            };

            _enrollmentRepository.Add(entity);
            return entity;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetByInternAsync(int internId)
        {
            return await _enrollmentRepository
                .GetAll()
                .Where(e => e.InternId == internId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    InternName = e.Intern != null ? e.Intern.FullName : string.Empty,
                    TrackId = e.TrackId,
                    TrackName = e.Track != null ? e.Track.Name : string.Empty,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int enrollmentId, EnrollmentStatus newStatus)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null) return false;

            enrollment.Status = newStatus;
            _enrollmentRepository.Update(enrollment);
            return true;
        }

        public async Task<bool> UpdateTrackAsync(int enrollmentId, int newTrackId)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null) return false;

            enrollment.TrackId = newTrackId;
            _enrollmentRepository.Update(enrollment);
            return true;
        }
    }
}
