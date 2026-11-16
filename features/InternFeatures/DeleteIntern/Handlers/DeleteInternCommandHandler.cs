using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.DeleteIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.DeleteIntern.Handlers;

public class DeleteInternCommandHandler(IGeneralRepository<Intern> repository,IUnitOfWork unitOfWork) :
    IRequestHandler<DeleteInternCommand, RequestResponse<bool>>
{
    public async Task<RequestResponse<bool>> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
    {
        var intern = await repository.GetAll().
                       Include(x => x.Enrollments)
                       
                       .FirstOrDefaultAsync(x => x.Id == request.id,cancellationToken);
        
        if(intern is null)
        {
            return RequestResponse<bool>.Fail(404, new Dictionary<string, string[]>
            {
                ["intern"] = ["intern not found"]
            });
        }

        
        // if intern has Enrolle and his ststus exist
        if (intern.Enrollments.Any())
        {

            return RequestResponse<bool>.Fail(409, new Dictionary<string, string[]>
            {
                ["Intern"] = ["Cannot delete intern because they have enrollment history."]
            });

        }
        repository.Delete(intern);
        
        await unitOfWork.SaveChangesAsync();
        return RequestResponse<bool>.OK(true,200);
    }
}
