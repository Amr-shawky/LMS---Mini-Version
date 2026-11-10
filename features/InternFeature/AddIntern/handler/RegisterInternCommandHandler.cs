using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.AddIntern.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.handler
{
    public class RegisterInternCommandHandler : IRequestHandler<RegisterInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterInternCommandHandler(
            IGeneralRepository<Intern> internRepository,
            IGeneralRepository<Track> trackRepository,
            IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(RegisterInternCommand request, CancellationToken cancellationToken)
        {

            // 1. Check if Track exists
            var track = await _trackRepository.GetByIdAsync(request.TrackId);
            if(track is null)
            {
                return RequestResponse<int>.Fail(404);
            }
            // 2. Check Track Is Active
            if(!track.IsActive)
            {
                return RequestResponse<int>.Fail(409);
            }
            // 3. Check if Email already exists (case-insensitive)
            var email = request.Email.Trim().ToLower();
            var IsEmailExist = await _internRepository.GetAll().AnyAsync
                (
                intern => intern.Email.ToLower() == email,
                cancellationToken
                );

            if(IsEmailExist)
            {
                return RequestResponse<int>.Fail(409);
            }
            // 4. Create new Intern
            var newIntern = new Intern()
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };

            // Add Intern
            _internRepository.Add(newIntern);

            // SaveChanges
            await _unitOfWork.SaveChangesAsync();

            // 7. Return the new Intern ID
            return RequestResponse<int>.OK(newIntern.Id, 201);
        }
    }
}
