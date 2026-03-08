using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    /// <summary>
    /// [CQRS Assignment] Handler for GetEnrollmentsByInternQuery.
    /// YOUR TASK: Query the database for all enrollments belonging to a specific intern,
    /// map them to EnrollmentViewModel, and return the list.
    /// 
    /// HINTS:
    ///   - Use _enrollmentRepository.GetTable() to get IQueryable
    ///   - Use .Include(e => e.Track).Include(e => e.Intern) for navigation properties
    ///   - Filter with .Where(e => e.InternId == request.InternId)
    ///   - Use .ToListAsync(cancellationToken)
    ///   - Map using .ToDto().ToViewModel()
    /// </summary>
    public class GetEnrollmentsByInternQueryHandler
        : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentViewModel>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetEnrollmentsByInternQueryHandler(
            IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> Handle(
            GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            // ╔══════════════════════════════════════════════════════════════╗
            // ║  🎯 ASSIGNMENT: Implement this handler                      ║
            // ║                                                              ║
            // ║  Return all enrollments for request.InternId                ║
            // ║  Include Track and Intern navigation properties              ║
            // ║  Map each Enrollment → EnrollmentDto → EnrollmentViewModel  ║
            // ╚══════════════════════════════════════════════════════════════╝
            throw new NotImplementedException("TODO: Implement this handler for the CQRS assignment");
        }
    }
}
