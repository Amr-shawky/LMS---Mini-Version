using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record class CreateInternCommand(string fullName,string Email,int BirthYear,int trackid) : IRequest<int>;

    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, int>
    {
        private readonly IGeneralRepository<Intern> _InternRepository;
        private readonly IGeneralRepository<Track> _TrackRepository;

        public CreateInternCommandHandler(IGeneralRepository<Intern> InternRepository,IGeneralRepository<Track> trackrepository)
        {
            _InternRepository = InternRepository;
            _TrackRepository = trackrepository;
        }

        public async Task<int> Handle(CreateInternCommand command,CancellationToken cancellationToken)
        {
            var track =  _TrackRepository.GetById(command.trackid);
            if(track is null||!track.IsActive)
                throw new InvalidOperationException(
                "Track is inactive or doesn't exist");

            var emailexist = await _InternRepository.GetTable().AnyAsync(i => i.Email == command.Email,cancellationToken);
            if (emailexist)
                throw new InvalidOperationException(
                    "Email already registered");

            var newintern = new Intern
            {
                FullName = command.fullName,
                Email = command.Email,
                BirthYear = command.BirthYear,
                TrackId = command.trackid,
                Status = "Pending"
            };

            _InternRepository.Add(newintern);
            return newintern.Id;
        }
    }
    
}
