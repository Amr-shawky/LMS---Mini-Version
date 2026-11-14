using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.Commands.handler
{
    public class AddInternCommandHandler : IRequestHandler<AddInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _internrepository;
        private readonly IGeneralRepository<Track> _trackrepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddInternCommandHandler(IGeneralRepository<Intern> internrepository, IGeneralRepository<Track> trackrepository, IUnitOfWork unitOfWork)
        {
            _internrepository = internrepository;
            _trackrepository = trackrepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<int>> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            // Check if the track exists
            var track = await _trackrepository.GetByIdAsync(request.TrackId);
            if (track == null)
            {
                return RequestResponse<int>.Fail(404,
            new Dictionary<string, string[]>
            {
                { "TrackId", new[] { "Track not found." } }
            });
            }
            // Check if the track active
            if (!track.IsActive)
            {
                return RequestResponse<int>.Fail(400,
            new Dictionary<string, string[]>
            {
                { "TrackId", new[] { "Track is not active." } }
            });
            }
            //check for email dupe
            var email = request.Email.Trim();
            var existingIntern = await _internrepository.GetAll().AnyAsync(E => E.Email.ToLower() == email.ToLower(), cancellationToken);
            if (existingIntern)
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    { "Email", new[] { "Email already exists." } }
                });

            }
            //Add the intern
            var intern = new Intern
            {
                FullName = request.FullName,
                Email = email,
                BirthYear = request.BirthYear,
                Status = "Pending",
                TrackId = request.TrackId
            };
            //saving
            _internrepository.Add(intern);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(intern.Id);
        }
    }
}
