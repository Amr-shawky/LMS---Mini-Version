using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record DeleteUnsendInternRecordCommand(int internId) : IRequest<RequestResponse>;

    public class DeleteUnsendInternRecordCommandHandler : IRequestHandler<DeleteUnsendInternRecordCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepo;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public DeleteUnsendInternRecordCommandHandler(IGeneralRepository<Intern> internRepo, IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _internRepo = internRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse> Handle(DeleteUnsendInternRecordCommand command,CancellationToken cancellationToken)
        {
            var intern = _internRepo.GetById(command.internId);
            if (intern is null)
                return new RequestResponse
                {
                    StatusCode = 404,
                    Success = false
                };

            var internEnrollments = await _enrollmentRepo.GetTable().Where(e => e.InternId == command.internId).FirstOrDefaultAsync();
            if (internEnrollments is not null)
                return new RequestResponse
                {
                    StatusCode = 409,
                    Success = false
                };

            _internRepo.Delete(command.internId);
            return RequestResponse.OK();
        }
    }
}
