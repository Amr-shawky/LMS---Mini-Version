using LMS___Mini_Version.CQRS.Interns.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Handler
{
    public class CreateNewInternCommandHandler : IRequestHandler<CreateNewInternCommand, RequestResult<bool>>
    {
        public IGeneralRepository<Intern> _repositiory;
        public CreateNewInternCommandHandler(IGeneralRepository<Intern> repositiory)
        {
            _repositiory = repositiory;
        }
        public async Task<RequestResult<bool>> Handle(CreateNewInternCommand request, CancellationToken cancellationToken)
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

            return RequestResult<bool>.Success(true);
        }
    }
}
