using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{
    /// <summary>
    /// [Trap 5 Fix] Single-entity Steps for Enrollment.
    /// [Trap 6 Fix] Only stages changes — no SaveChanges. The Mediator commits via UoW.
    /// </summary>
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => e.ToDto());
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == id)
                .ConfigureAwait(false);

            return enrollment?.ToDto();
        }

        /// <summary>
        /// Stages a new Enrollment entity in the Change Tracker.
        /// Does NOT call SaveChanges — the Mediator will call UoW.CompleteAsync()
        /// after all steps (enrollment + payment) are staged.
        /// </summary>
        public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto dto)
        {
            var entity = new Enrollment
            {
                InternId = dto.InternId,
                TrackId = dto.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending
            };

            _unitOfWork.Enrollments.Add(entity);
            return entity.ToDto();
        }

        public async Task<IEnumerable<EnrollmentDto>> GetByInternAsync(int internId)
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetTable()
                .Include(e => e.Track)
                .Include(e => e.Intern)
                .Where(e => e.InternId == internId)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(e => e.ToDto());
        }
    }
}
