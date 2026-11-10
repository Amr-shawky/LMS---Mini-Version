using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern.Handler
{
    public class DeleteInternHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var internId = request.DeleteInternDTO.InterId;
            var internRepo = unitOfWork.GetRepository<Intern>();
            var intern = await internRepo.GetByIdAsync(internId);
            if (intern == null)
                return RequestResponse.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["InternId"] = [$"Intern with Id {internId} is NotFound"] });

            var hasEnrollments = await unitOfWork.GetRepository<Enrollment>().GetAll()
                .AnyAsync(e => e.InternId == internId, cancellationToken: cancellationToken);

            if (hasEnrollments)
            {
                return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["InternEnrollment"] = [$"Intern Must Have No Enrollment History"] });
            }
            internRepo.Delete(intern);
            //await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();

        }
    }
}
