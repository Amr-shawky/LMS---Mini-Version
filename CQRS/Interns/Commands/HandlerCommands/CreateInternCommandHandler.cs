using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands.HandlerCommands;

public class CreateInternCommandHandler(IGeneralRepository<Domain.Entities.Intern> _internRepo)
    : IRequestHandler<CreateInternCommand, InternDto>
{    
    public async Task<InternDto> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {

        /// we will checl validation later 
        var intern = new Domain.Entities.Intern 
        {
            FullName = request.fullname,
            Email = request.email,
            BirthYear = request.birthYear,
            Status = request.state,
            TrackId = request.trackId
        };
        /// need to check if this track is found or not
        /// 
        //var track = await _unitOfWork.Tracks.GetByIdAsync(request.trackId);
        var track = await _internRepo.GetByIdAsync(request.trackId);

        if (track == null)
        {
            // this line need refactor to return bad request with message instead of throwing exception
            throw new Exception($"this trackID {request.trackId} Not Found"); 
        }
        else 
        {
            _internRepo.Add(intern);

            return intern.ToDto();
        }
      
    }
}
