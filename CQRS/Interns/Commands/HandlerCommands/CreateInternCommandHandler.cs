using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using LMS___Mini_Version.CQRS.Interns.Commands;
using LMS___Mini_Version.Mapping;

namespace LMS___Mini_Version.CQRS.Interns.Commands.HandlerCommands
{
    public class CreateInternCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<CreateInternCommand, CreateInternViewModel>
    {

        public async Task<CreateInternViewModel> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            
            var newIntern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = request.Status
            };

            _unitOfWork.Interns.Add(newIntern);
           await _unitOfWork.CompleteAsync();

            return new CreateInternViewModel
            {

                FullName = newIntern.FullName,
                Email = newIntern.Email,
                BirthYear = newIntern.BirthYear,
                TrackId = newIntern.TrackId,
                Status = newIntern.Status
            };
        }
    }
}
