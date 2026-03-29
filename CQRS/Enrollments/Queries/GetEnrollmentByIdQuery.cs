using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries
{
    public record GetEnrollmentByIdQuery(int id) : IRequest<EnrollmentDto?>;

    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto?>
    {
        private readonly IEnrollmentService _enrollmentService;

        public GetEnrollmentByIdQueryHandler(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public async Task<EnrollmentDto?> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentService.GetByIdAsync(request.id);
        }
    }
}
