using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class GetInternsWithTrackQueryHandler : IRequestHandler<GetInternsWithTrackQuery, RequestResponse<IEnumerable<InternsWithTrackDTO>>>
    {
        private readonly IGeneralRepository<Intern> internrepo;

        public GetInternsWithTrackQueryHandler(IGeneralRepository<Intern> internrepo)
        {
            this.internrepo = internrepo;
        }
        public async Task<RequestResponse<IEnumerable<InternsWithTrackDTO>>> Handle(GetInternsWithTrackQuery request, CancellationToken cancellationToken)
        {
          var res =  await  internrepo.GetAll().Select(x => new InternsWithTrackDTO{ 
                TrackName = x.Track.Name,
                Id = x.Id,
                Email = x.Email,
                FullName = x.FullName,
                Status = x.Status   
            }).ToListAsync();

            return res.Any() ? RequestResponse<IEnumerable<InternsWithTrackDTO>>.OK(res, 200) : RequestResponse<IEnumerable<InternsWithTrackDTO>>.OK([], 200);

            
           
        }
    }
}
