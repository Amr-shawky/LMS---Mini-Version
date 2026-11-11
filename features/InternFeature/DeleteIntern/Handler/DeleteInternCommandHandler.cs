using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern.Handler
{
    public class DeleteInternCommandHandler
        : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteInternCommandHandler(
            IGeneralRepository<Intern> repository,
            IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(
            DeleteInternCommand request,
            CancellationToken cancellationToken)
        {
            var intern = await repository.GetByIdAsync(request.InternId);

            if (intern is null)
            {
                return RequestResponse.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["InternId"] = new[] { "Intern not found." }
                    });
            }

            var hasEnrollments = await repository.GetAll()
                .AnyAsync(
                    i => i.Id == request.InternId &&
                         i.Enrollments.Any(),
                    cancellationToken);

            if (hasEnrollments)
            {
                return RequestResponse.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["Enrollments"] = new[]
                        {
                            "Cannot delete an intern with enrollment history."
                        }
                    });
            }

            repository.Delete(intern);

            await unitOfWork.SaveChangesAsync();

            return RequestResponse.OK(200);
        }
    }
}
