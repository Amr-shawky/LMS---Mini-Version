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
            var enrollments = await _enrollmentRepository
                .GetAll()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                InternId = e.InternId,
                InternName = e.Intern?.FullName ?? string.Empty,
                TrackId = e.TrackId,
                TrackName = e.Track?.Name ?? string.Empty,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            });
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            var enrollment = await _enrollmentRepository
                .GetAll()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);

            return enrollment is null ? null : new EnrollmentDto
            {
                Id = enrollment.Id,
                InternId = enrollment.InternId,
                InternName = enrollment.Intern?.FullName ?? string.Empty,
                TrackId = enrollment.TrackId,
                TrackName = enrollment.Track?.Name ?? string.Empty,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status
            };
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
            var enrollments = await _enrollmentRepository
                .GetAll()
                .Include(e => e.Track)
                .Include(e => e.Intern)
                .Where(e => e.InternId == internId)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                InternId = e.InternId,
                InternName = e.Intern?.FullName ?? string.Empty,
                TrackId = e.TrackId,
                TrackName = e.Track?.Name ?? string.Empty,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            });
        }

        public async Task<bool> UpdateStatusAsync(int enrollmentId, EnrollmentStatus newStatus)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId).ConfigureAwait(false);
            if (enrollment == null) return false;

            enrollment.Status = newStatus;
            _enrollmentRepository.Update(enrollment);
            return true;
        }

        public async Task<bool> UpdateTrackAsync(int enrollmentId, int newTrackId)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId).ConfigureAwait(false);
            if (enrollment == null) return false;

            enrollment.TrackId = newTrackId;
            _enrollmentRepository.Update(enrollment);
            return true;
        }
    }
}
