using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepository;
        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepository = enrollmentrepo;

        }
        public async Task<EnrollmentDto> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollmententity =await _enrollmentrepository.GetByIdAsync(request.EnrollmentId);

            EnrollmentDto enrollmentdto = new EnrollmentDto
            {
                Id = enrollmententity.Id,
                InternId = enrollmententity.InternId,
                TrackId = enrollmententity.TrackId,
                EnrollmentDate = enrollmententity.EnrollmentDate,
                Status = enrollmententity.Status
            };

            return enrollmentdto;
        }
    }
}
