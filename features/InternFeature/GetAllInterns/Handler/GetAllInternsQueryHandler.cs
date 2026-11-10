using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.DTOS;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Handler
{
    public class GetAllInternsQueryHandler(IUnitOfWork unitOfWork) :
        IRequestHandler<GetAllInternsQuery, RequestResponse<List<GetAllInternsDTO>>>
    {
        public async Task<RequestResponse<List<GetAllInternsDTO>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var internRepo = unitOfWork.GetRepository<Domain.Entities.Intern>();
            //Why GetAll And Not GetAllAsync
            //because GetAll Will allow the Service to extend the filtering operations and its not its job 
            var interns = await internRepo.GetAll()
             .OrderBy(i => i.Id)
             .Select(s => new GetAllInternsDTO(s.Id, s.FullName, s.Email, s.Track.Name, s.Status))
             .ToListAsync(cancellationToken);

            return RequestResponse<List<GetAllInternsDTO>>.OK(interns);

        }
    }
}
