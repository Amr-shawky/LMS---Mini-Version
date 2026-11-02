using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.AddIntern.Command
{
    public record AddInternCommand(string fullName,string email,int birthYear,string status,int trackId) : IRequest<bool>;

    public class AddInternCommandHandler : IRequestHandler<AddInternCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddInternCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            var intern = new LMS___Mini_Version.Domain.Entities.Intern()
            {
                BirthYear = request.birthYear,
                Email = request.email,
                FullName = request.fullName,
                Status = request.status,
                TrackId = request.trackId
            };
           await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Intern, int>().AddAsync(intern);
           await _unitOfWork.SaveChagesAsync();
            return true;
        }
    }
}
