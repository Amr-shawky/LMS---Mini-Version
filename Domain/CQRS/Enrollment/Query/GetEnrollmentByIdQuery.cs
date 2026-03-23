using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Enrollment.Query
{
    public record GetEnrollmentByIdQuery(int EnrollId):IRequest<EnrollmentSummaryDTO?>;

    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentSummaryDTO?>
    {
        private readonly IGeneralRepository<Entities.Enrollment> _enrollmentRepository;
        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Entities.Enrollment> repository)
        {
            _enrollmentRepository = repository;
        }
        public async Task<EnrollmentSummaryDTO?> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
           var Enroll=await _enrollmentRepository.GetById(request.EnrollId);
            if (Enroll == null) return null;
            return new EnrollmentSummaryDTO()
            {
                Id = Enroll.Id,
                status = Enroll.Status.ToString()
            };
        }
    }




}
