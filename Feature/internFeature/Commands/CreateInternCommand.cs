using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.internFeature.Commands
{
    public record CreateInternCommand(string FullName, string Email, int BirthYear, string Status, int TrackId) : IRequest<RequestResponse<int>>;

    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public CreateInternCommandHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                Status = request.Status,
                TrackId = request.TrackId
            };

            _internRepository.Add(intern);
            await _internRepository.SaveChangesAsync();

            return RequestResponse<int>.Created(intern.Id);
        }
    }
}
