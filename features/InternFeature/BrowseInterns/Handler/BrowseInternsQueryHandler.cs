using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.BrowseInterns.Dtos;
using LMS___Mini_Version.features.InternFeature.BrowseInterns.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS___Mini_Version.features.InternFeature.BrowseInterns.Handler
{
    public class BrowseInternsQueryHandler : IRequestHandler<BrowseInternsQuery, RequestResponse<IEnumerable<InternResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Intern> _internRepository;

        public BrowseInternsQueryHandler(IUnitOfWork unitOfWork , IGeneralRepository<Intern> internRepository)
        {
            _unitOfWork = unitOfWork;
            _internRepository = internRepository;
        }
        public async Task<RequestResponse<IEnumerable<InternResponseDto>>> Handle(BrowseInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _internRepository.GetAll().AsNoTracking().Select
                (
                   x => new InternResponseDto(
                       x.Id,
                       x.FullName,
                       x.Email,
                       x.Status,
                       x.Track.Name
                   )
                ).ToListAsync();

            return RequestResponse<IEnumerable<InternResponseDto>>.OK(interns, StatusCodes.Status200OK);
        }
    }
}
