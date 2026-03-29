using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries
{
    public record GetEnrollmentsByInternQuery(int internId) : IRequest<IEnumerable<EnrollmentDto>>;

    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IEnrollmentService _enrollmentService;

        public GetEnrollmentsByInternQueryHandler(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentService.GetByInternAsync(request.internId);
        }
    }
}
