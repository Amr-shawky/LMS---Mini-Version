using Azure.Core;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.RegisterIntern
{
    public class RegisterInternHandler(IGeneralRepository<Intern> _internRepo, IGeneralRepository<Track> _trackRepo,IUnitOfWork unitOfWork) : IRequestHandler<RegisterInternCommand, RequestResponse<int>>
    {
       
        public async Task<RequestResponse<int>> Handle(RegisterInternCommand request, CancellationToken cancellationToken)
        {
            Track track = _trackRepo.GetAll().FirstOrDefault(t => t.Id == request.trackId);
            Intern intern = _internRepo.GetAll().FirstOrDefault(i => i.Email == request.email);
            List<string> errors = new List<string>();

            if (track == null)
                errors.Add("The track does not exist");
            else if(track.IsActive == false)
                errors.Add("The track is not active");
            if (intern != null)
                errors.Add("The intern already exists");

            if (errors.Any() )
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    { "ValidationErrors", errors.ToArray() }
                });
            Intern newIntern = new Intern { FullName = request.fullName, Email = request.email, BirthYear = request.birthYear, TrackId = request.trackId, Status = "Pending" };

           _internRepo.Add(newIntern );

            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(newIntern.Id, 201);

        }
    }
}
