using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern.Handler
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(IGeneralRepository<Intern> internRepository , IGeneralRepository<Enrollment> enrollmentRepository, IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if Intern exists
            var intern = await _internRepository.GetByIdAsync(request.internId);
            if(intern is null)
            {
                return RequestResponse.Fail(400, new Dictionary<string, string[]> { { "InternId", new[] { "Intern Not Found" } } });
            }
            // 2. Check if Intern has any Enrollment
            var EnrollmentExist = await _enrollmentRepository.GetAll().AnyAsync(x => x.InternId == request.internId , cancellationToken);
            if(EnrollmentExist)
            {
                return RequestResponse.Fail(
                409,
                new Dictionary<string, string[]>
                {
                   {
                       "InternId",
                       new[] { "Cannot delete an intern who has enrollment history." }
                   }
                });
            }

            _internRepository.Delete(intern);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
