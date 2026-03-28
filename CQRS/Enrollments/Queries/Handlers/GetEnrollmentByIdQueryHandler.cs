using LMS___Mini_Version.CQRS.Enrollments.Queries.Requests;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Handlers
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto>
    {
        private readonly IUnitOfWork _unitOf;
        public GetEnrollmentByIdQueryHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<EnrollmentDto> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOf.Enrollments.GetByIdAsync(request.EnrollmentId);
            if (enrollment == null)
                throw new InvalidOperationException($"enrollment with Id {request.EnrollmentId} Doesn't exist");

            return enrollment.ToDto();
        }
    }
}

