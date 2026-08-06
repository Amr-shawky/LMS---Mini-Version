using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{
    /// <summary>
    /// [Trap 3 Fix] Fully async.
    /// [Trap 4 Fix] Uses IQueryable with Include for DB-side joins.
    /// [Trap 5 Fix] Single-entity Steps only.
    /// [Trap 6 Fix] No SaveChanges.
    /// </summary>
    public class InternService : IInternService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InternService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InternDto>> GetAllAsync()
        {
            var interns = await _unitOfWork.Interns
                .GetTable()
                .Include(i => i.Track)
                .ToListAsync()
                .ConfigureAwait(false);

            return interns.Select(i => i.ToDto());
        }

        public async Task<InternDto?> GetByIdAsync(int id)
        {
            // [Trap 4 Fix] Use IQueryable + Include to load Track name in the same SQL query
            var intern = await _unitOfWork.Interns
                .GetTable()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id == id)
                .ConfigureAwait(false);

            return intern?.ToDto();
        }

        public async Task<InternDto> CreateAsync(InternDto dto)
        {
            var entity = new Domain.Entities.Intern
            {
                FullName = dto.FullName,
                Email = dto.Email,
                BirthYear = dto.BirthYear,
                Status = dto.Status,
                TrackId = dto.TrackId
            };

            _unitOfWork.Interns.Add(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateAsync(int id, InternDto dto)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(id).ConfigureAwait(false);
            if (intern == null) return false;

            intern.FullName = dto.FullName;
            intern.Email = dto.Email;
            intern.BirthYear = dto.BirthYear;
            intern.Status = dto.Status;
            intern.TrackId = dto.TrackId;

            _unitOfWork.Interns.Update(intern);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(id).ConfigureAwait(false);
            if (intern == null) return false;

            _unitOfWork.Interns.Delete(intern);
            return true;
        }
    }
}
