using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    /// <summary>
    /// [CQRS Assignment] Handler for GetAllInternsQuery.
    /// YOUR TASK: Query the database for all interns (include Track navigation),
    /// map them to InternSummaryViewModel, and return the list.
    /// 
    /// HINTS:
    ///   - Use _internRepository.GetTable() to get IQueryable
    ///   - Use .Include(i => i.Track) to load the Track navigation property
    ///   - Use .ToListAsync(cancellationToken)
    ///   - Map using .ToDto().ToSummaryViewModel()
    /// </summary>
    public class GetAllInternsQueryHandler
        : IRequestHandler<GetAllInternsQuery, IEnumerable<InternSummaryViewModel>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<IEnumerable<InternSummaryViewModel>> Handle(
            GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            // ╔══════════════════════════════════════════════════════════════╗
            // ║  🎯 ASSIGNMENT: Implement this handler                      ║
            // ║                                                              ║
            // ║  Return ALL interns with their Track info included           ║
            // ║  Map each Intern entity → InternDto → InternSummaryViewModel║
            // ╚══════════════════════════════════════════════════════════════╝
            throw new NotImplementedException("TODO: Implement this handler for the CQRS assignment");
        }
    }
}
