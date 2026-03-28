using LMS___Mini_Version.CQRS.Enrollments.Queries.Requests;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Handlers
{
    public class GetActiveEnrollmentByInternIdQueryHandler : IRequestHandler<GetActiveEnrollmentByInternIdQuery, EnrollmentDto>
    {

        private readonly IUnitOfWork _unitOf;
        public GetActiveEnrollmentByInternIdQueryHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<EnrollmentDto> Handle(GetActiveEnrollmentByInternIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _unitOf.Interns.GetByIdAsync(request.InternId);
            if (intern == null)
                throw new InvalidOperationException($"Intern with Id {request.InternId} Doesn't exist");

            var enrollment = await _unitOf.Enrollments.GetActiveEnrollmentByInternIdAsync(request.InternId);
            if (enrollment == null)
                throw new InvalidOperationException($"This Intern with Id {request.InternId} doesn't have an enrollment");

            return enrollment.ToDto();
        }

    }
}


