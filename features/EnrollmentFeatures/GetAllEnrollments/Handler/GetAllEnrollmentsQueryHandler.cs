using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollmentsHistory.GetEnrollmentsHistoryQuery;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Handler
{
    public class GetAllEnrollmentsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository
        ,IGeneralRepository<Intern> internrepository) : IRequestHandler<GetAllEnrollmentsQuery, RequestResponse<IEnumerable<EnrollmentDto>>>
    {
        private readonly IGeneralRepository<Enrollment> enrollmentRepository = enrollmentRepository;
        private readonly IGeneralRepository<Intern> internrepository = internrepository;

        public async Task<RequestResponse<IEnumerable<EnrollmentDto>>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {

            
            var InternExists = await internrepository.GetAll().AnyAsync(x=>x.Id==request.InternId);
            if(!InternExists)
            {
                return RequestResponse<IEnumerable<EnrollmentDto>>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern not found" }
                });
            }

            var EnrollmentsDto = await enrollmentRepository.GetAll()
                .Where(x => x.InternId == request.InternId)
                .OrderByDescending(x=>x.EnrollmentDate)
                .ThenByDescending(x=>x.Id)
                .Select(x => new EnrollmentDto
                {
                    EnrollmentId = x.Id,
                    EnrollmentDate = x.EnrollmentDate,
                    EnrollmentStatus = x.Status,
                    TrackId = x.TrackId,
                    TrackName = x.Track.Name
                }).ToListAsync();
            return RequestResponse<IEnumerable<EnrollmentDto>>.OK(EnrollmentsDto);
        }
    }
}
