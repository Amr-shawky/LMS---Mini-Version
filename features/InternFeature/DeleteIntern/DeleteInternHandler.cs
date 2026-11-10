using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern
{
    public class DeleteInternHandler : IRequestHandler<DeleteInternCommand, RequestResponse>
    {
        readonly private IGeneralRepository<Intern> _internRepository;
        readonly private IUnitOfWork _unitOfWork;

        public DeleteInternHandler(IGeneralRepository<Intern> internRepository, IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _unitOfWork = unitOfWork;
          
        }

        public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            Intern intern = _internRepository.GetAll().FirstOrDefault(i => i.Id == request.internId);
            if(intern is null)
                return RequestResponse.Fail(404, ErrorCode.InternNotFound);
            bool IsEnrolled = _internRepository.GetAll().Any(i => i.Id == request.internId && i.Enrollments.Any());            
            if(IsEnrolled)
                return RequestResponse.Fail(400, ErrorCode.InternHasEnrollment);

            _internRepository.Delete(intern);
            _unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();


        }
    }
    
}
