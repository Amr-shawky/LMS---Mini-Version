using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.DeleteIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.DeleteIntern.Handlers
{
    public class DeleteInternCommandHandler(IGeneralRepository<Intern> internRepository,
        IGeneralRepository<Enrollment> enrollmentRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> internRepository = internRepository;
        private readonly IGeneralRepository<Enrollment> enrollmentRepository = enrollmentRepository;

        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await internRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.internId);
            

            if (intern ==null)
            {
                return  RequestResponse.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.internId)] = new [] { "Intern not found" }
                });
            }
            var hasEnrollments = await enrollmentRepository.GetAll().AnyAsync(x => x.InternId == request.internId);
            if (hasEnrollments)
            {
                return  RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.internId)] = new [] { "Intern has enrollments" }
                });
            }
            internRepository.Delete(intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();
        }
    }
}
