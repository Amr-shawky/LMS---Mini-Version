using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.CreateIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.CreateIntern.Handlers;

public class CreateInternCommandHandler :
    IRequestHandler<CreateInternCommand, RequestResponse<int>>
{
    private readonly IGeneralRepository<Intern> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeneralRepository<Track> _generalRepository;

    public CreateInternCommandHandler
        (IGeneralRepository<Intern> repository,
        IUnitOfWork unitOfWork, IGeneralRepository<Track> generalRepository) 
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _generalRepository = generalRepository;
    }
    public async Task<RequestResponse<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {

        //check active track
        var track = await _generalRepository.GetByIdAsync(request.TrackId);
        if (track is null)
        {
             return RequestResponse<int>.Fail(404 , new Dictionary<string, string[]>
             {
                 ["trackid"]=new[] { "track not found" }
             });
            
        }

        if (!track.IsActive)
        {
            return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
            {
                ["isActive"] = new[] { "track not active" }
            });
        }


        //unieq Email
        var email = request.Email.Trim().ToLower();
        var IsUnique = _repository.GetAll().Any(x => x.Email.ToLower() == email);
        if (IsUnique)
        {
            return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
            {
                ["IsUniqueEmail"] = new[] { "Enter valid email" }
            });
        }

        var intern  = new Intern { 
        
            FullName = request.FullName,
            Email =request.Email,
            BirthYear = request.BirthYear,
            TrackId = request.TrackId,
            Status = "Pending"
            

        };
        

        _repository.Add(intern);
       await _unitOfWork.SaveChangesAsync();
        return RequestResponse<int>.OK(intern.Id,200);
    }
}
