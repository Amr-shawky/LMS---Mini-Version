namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Handlers
{
    public class GetEnrollmentsByInternIdHandler : IRequestHandler<GetEnrollmentsByInternId, IEnumerable<EnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOf;
        public GetEnrollmentsByInternIdHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternId request, CancellationToken cancellationToken)
        {
            var intern = _unitOf.Interns.GetByIdAsync(request.InternId);
            if (intern == null)
                throw new InvalidOperationException($"Intern with Id {request.InternId} Doesn't exist");

            var enrollments = await _unitOf.Enrollments.GetAllByInternIdAsync(request.InternId);
            return enrollments.Select(e => e.ToDto());
        }

    }
}

