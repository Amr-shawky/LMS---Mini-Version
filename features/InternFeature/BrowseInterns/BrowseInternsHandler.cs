using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.BrowseInterns
{
    public class BrowseInternsHandler : IRequestHandler<BrowseInternsQuery, RequestResponse<IEnumerable<BrowseInternsDto>>>
    {
        readonly private IGeneralRepository<Intern>  _generalRepository;
        public BrowseInternsHandler(IGeneralRepository<Intern> generalRepository)
        {

            _generalRepository = generalRepository;
        }

        public async Task<RequestResponse<IEnumerable<BrowseInternsDto>>> Handle(BrowseInternsQuery request, CancellationToken cancellationToken)
        {

            IEnumerable<BrowseInternsDto>? internsDto = _generalRepository.GetAll().Select(
                i => new BrowseInternsDto
                {
                    InternId = i.Id,
                    FullName = i.FullName,
                    Email = i.Email,
                    Status = i.Status,
                    TrackName = i.Track.Name
                });
            

            return RequestResponse<IEnumerable<BrowseInternsDto>>.OK(internsDto);

        }
    }
}
