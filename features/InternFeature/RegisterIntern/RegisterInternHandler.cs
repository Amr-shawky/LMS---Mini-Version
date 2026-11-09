using Azure.Core;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.RegisterIntern
{
    public class RegisterInternHandler(IGeneralRepository<Intern> _internRepo, IGeneralRepository<Track> _trackRepo,IUnitOfWork unitOfWork) : IRequestHandler<RegisterInternCommand, RequestResponse<RegisterDto>>
    {
       
        public async Task<RequestResponse<RegisterDto>> Handle(RegisterInternCommand request, CancellationToken cancellationToken)
        {
            Track track = _trackRepo.GetAll().FirstOrDefault(t => t.Id == request.trackId);
            Intern intern = _internRepo.GetAll().FirstOrDefault(i => i.Email == request.email);
            

            if (track == null)
                return RequestResponse<RegisterDto>.Fail(404, ErrorCode.TrackNotFound);
            else if(track.IsActive == false)
                return RequestResponse<RegisterDto>.Fail(400, ErrorCode.TrackIsNotActive);
            if (intern != null)
                return RequestResponse<RegisterDto>.Fail(400, ErrorCode.InternAlreadyExists);


            Intern newIntern = new Intern { FullName = request.fullName, Email = request.email, BirthYear = request.birthYear, TrackId = request.trackId, Status = "Pending" };

           _internRepo.Add(newIntern);

            await unitOfWork.SaveChangesAsync();
            return RequestResponse<RegisterDto>.OK(new RegisterDto { internId= newIntern.Id}, 201);

        }
    }
}
