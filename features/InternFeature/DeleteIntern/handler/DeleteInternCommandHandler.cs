using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern.handler
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(
            IGeneralRepository<Intern> internRepository,
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetAll()
                .FirstOrDefaultAsync(i => i.Id == request.InternId, cancellationToken);

            if (intern is null)
                return RequestResponse.Fail(StatusCodes.Status404NotFound,
                    "InternId", $"Intern with id {request.InternId} was not found.");

            // Any enrollment (Pending, Active, Completed or Cancelled) is history that must be kept.
            // We check explicitly instead of relying on the FK restriction to throw.
            var enrollmentCount = await _enrollmentRepository.GetAll()
                .CountAsync(e => e.InternId == request.InternId, cancellationToken);

            if (enrollmentCount > 0)
                return RequestResponse.Fail(StatusCodes.Status409Conflict, "InternId",
                    $"Intern '{intern.FullName}' has {enrollmentCount} enrollment record(s) and cannot be deleted. " +
                    "Enrollment history must be preserved.");

            _internRepository.Delete(intern);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
