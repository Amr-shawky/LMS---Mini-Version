using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Command.DeleteIntern;

public class DeleteInternCommandHandler(IGeneralRepository<Intern> internRepository ,
    IGeneralRepository<Enrollment>  enrollmentRepository , IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteInternCommand , RequestResponse>
{
    public async Task<RequestResponse> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {
        // get intern 
        var intern = await internRepository.GetByIdAsync(request.InternId);
        if (intern is null)
            return RequestResponse.Fail(404, new Dictionary<string, string[]>
            {
                { "InternId", ["Intern not found"] }
            });
        
        // check if intern has enrollments
        var hasEnrollments = await enrollmentRepository.GetAll()
            .AnyAsync(e => e.InternId == request.InternId, cancellationToken);
        if (hasEnrollments)
            return RequestResponse.Fail(409, new Dictionary<string, string[]>
            {
                { "Intern", ["Intern has enrollment history and cannot be deleted"] }
            });
        
        // 3. delete
        internRepository.Delete(intern);   
        await unitOfWork.SaveChangesAsync();

        return RequestResponse.OK();
        
    }
}