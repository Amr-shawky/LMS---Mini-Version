using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GatAllInternsQueryHandler : IRequestHandler<GetAllIntentsQuery, IEnumerable<InternSummaryViewModel>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GatAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }
        public async Task<IEnumerable<InternSummaryViewModel>> Handle(GetAllIntentsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _internRepository
                .GetTable()
                .Include(i => i.Track)
                .ToListAsync();

            return interns.Select(i => i.ToDto().ToSummaryViewModel());
        }
    }
}
