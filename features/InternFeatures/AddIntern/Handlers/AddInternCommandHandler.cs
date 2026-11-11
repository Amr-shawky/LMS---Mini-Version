using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.AddIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.AddIntern.Handlers
{
    public class AddInternCommandHandler(IGeneralRepository<Intern> Internrepository,IGeneralRepository<Track> Trackrepository, IUnitOfWork unitOfWork) : IRequestHandler<AddInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> InternRepository = Internrepository;

        private readonly IGeneralRepository<Track> trackRepository = Trackrepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;



        public async Task<RequestResponse> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            var track = await trackRepository.GetByIdAsync(request.TrackId);
            if (track == null)
            {
                return RequestResponse.Fail(404,new Dictionary<string, string[]> 
                {
                    [nameof(request.TrackId)] = new [] { "Track not found" }
                });
            }

            if(!track.IsActive)
            {
                return RequestResponse.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.TrackId)] = new[] { "Track is not active" }
                });
            }


            var emailExists = await InternRepository.GetAll().AnyAsync(i => i.Email == request.Email);
            if (emailExists)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.Email)] = new[] { "Email already exists" }
                });
            }

            var Intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };

        

            InternRepository.Add(Intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();
        }
    }
}
