using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeatures.GetInternById.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.GetInternById.Handler
{
    public class GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepo) : IRequestHandler<GetInternByIdQuery, RequestResponse<InternDto>>
    {
        private readonly IGeneralRepository<Intern> internRepo = internRepo;

        public async Task<RequestResponse<InternDto>> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var interndto = await internRepo.GetAll().Where(x=>x.Id==request.id).Select(x=> new InternDto 
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackId = x.TrackId,
                TrackName=x.Track.Name
            }).FirstOrDefaultAsync();
            if (interndto == null)
            {
                return RequestResponse<InternDto>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.id)] = new[] { "Intern not found" }
                });
            }
           
            return RequestResponse<InternDto>.OK(interndto);
        }
    }
}
