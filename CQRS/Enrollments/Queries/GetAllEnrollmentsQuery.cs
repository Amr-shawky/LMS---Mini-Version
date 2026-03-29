using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries
{
    public record GetAllEnrollmentsQuery : IRequest<IEnumerable<EnrollmentDto>>;

    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IEnrollmentService _enrollmentService;

        public GetAllEnrollmentsQueryHandler(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentService.GetAllAsync();
        }
    }
}
