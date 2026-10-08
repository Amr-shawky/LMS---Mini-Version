using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.internFeature.Commands
{
    public record UpdateInternCommand(int Id, string FullName, string Email, int BirthYear, string Status, int TrackId) : IRequest<RequestResponse>;

    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInternCommandHandler(IGeneralRepository<Intern> internRepository, IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetByIdAsync(request.Id);
            if (intern == null)
            {
                return RequestResponse.Fail($"Intern with ID {request.Id} not found", 404);
            }

            intern.FullName = request.FullName;
            intern.Email = request.Email;
            intern.BirthYear = request.BirthYear;
            intern.Status = request.Status;
            intern.TrackId = request.TrackId;

            _internRepository.Update(intern);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.Ok("Intern updated successfully");
        }
    }
}
