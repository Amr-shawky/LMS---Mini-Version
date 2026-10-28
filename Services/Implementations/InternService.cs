using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{

    public class InternService : IInternService
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InternService(
            IGeneralRepository<Intern> internRepository,
            IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InternDto>> GetAllAsync()
        {
            var interns = await _internRepository
                .GetAll()
                .Include(i => i.Track)
                .ToListAsync()
                ;

            return interns.Select(i => new InternDto
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                BirthYear = i.BirthYear,
                Status = i.Status,
                TrackId = i.TrackId,
                TrackName = i.Track?.Name ?? string.Empty
            });
        }

        public async Task<InternDto?> GetByIdAsync(int id)
        {
            var intern = await _internRepository
                .GetAll()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id == id)
                ;

            return intern is null ? null : new InternDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email = intern.Email,
                BirthYear = intern.BirthYear,
                Status = intern.Status,
                TrackId = intern.TrackId,
                TrackName = intern.Track?.Name ?? string.Empty
            };
        }

        public async Task<InternDto> CreateAsync(InternDto dto)
        {
            var entity = new Intern
            {
                FullName = dto.FullName,
                Email = dto.Email,
                BirthYear = dto.BirthYear,
                Status = dto.Status,
                TrackId = dto.TrackId
            };

            _internRepository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return new InternDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                BirthYear = entity.BirthYear,
                Status = entity.Status,
                TrackId = entity.TrackId,
                TrackName = entity.Track?.Name ?? dto.TrackName
            };
        }

        public async Task<bool> UpdateAsync(int id, InternDto dto)
        {
            var intern = await _internRepository.GetByIdAsync(id);
            if (intern == null) return false;

            intern.FullName = dto.FullName;
            intern.Email = dto.Email;
            intern.BirthYear = dto.BirthYear;
            intern.Status = dto.Status;
            intern.TrackId = dto.TrackId;

            _internRepository.Update(intern);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var intern = await _internRepository.GetByIdAsync(id);
            if (intern == null) return false;

            _internRepository.Delete(intern);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
