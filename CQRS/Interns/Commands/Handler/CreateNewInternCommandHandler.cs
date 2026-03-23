using LMS___Mini_Version.CQRS.Interns.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands.Handler
{
    public class CreateNewInternCommandHandler : IRequestHandler<CreateNewInternCommand, bool>
    {
        public IGeneralRepository<Intern> _repositiory;
        public CreateNewInternCommandHandler(IGeneralRepository<Intern> repositiory)
        {
            _repositiory = repositiory;
        }
        public Task<bool> Handle(CreateNewInternCommand request, CancellationToken cancellationToken)
        {
            var newIntern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                Status = request.Status,
                TrackId = request.TrackId
            };

            _repositiory.Add(newIntern);
            
            return Task.FromResult(true);
        }
    }
}
