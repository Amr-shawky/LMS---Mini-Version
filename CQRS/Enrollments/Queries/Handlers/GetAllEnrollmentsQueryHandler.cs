using LMS___Mini_Version.CQRS.Enrollments.Queries.Requests;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Handlers
{
    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<EnrollmentDto>>>
    {
        private readonly IUnitOfWork _unitOf;
        public GetAllEnrollmentsQueryHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _unitOf.Enrollments.GetAllAsync();
            return enrollments.Select(e => e.ToDto());
        }

    }
}

