using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.UpdateIntern
{
    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInternCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(request.id).ConfigureAwait(false);
            if (intern == null) return false;

            intern.FullName = request.dto.FullName;
            intern.Email = request.dto.Email;
            intern.BirthYear = request.dto.BirthYear;
            intern.Status = request.dto.Status;
            intern.TrackId = request.dto.TrackId;

            _unitOfWork.Interns.Update(intern);
            return true;
        }
    }
}
