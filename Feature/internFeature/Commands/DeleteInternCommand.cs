using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.internFeature.Commands
{
    public record DeleteInternCommand(int Id) : IRequest<RequestResponse>;

    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public DeleteInternCommandHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetByIdAsync(request.Id);
            if (intern == null)
            {
                return RequestResponse.Fail($"Intern with ID {request.Id} not found", 404);
            }

            _internRepository.Delete(intern);
            await _internRepository.SaveChangesAsync();

            return RequestResponse.Ok("Intern deleted successfully");
        }
    }
}
