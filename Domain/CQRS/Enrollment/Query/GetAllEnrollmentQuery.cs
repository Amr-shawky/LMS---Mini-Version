using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Enrollment.Query
{
    public record GetAllEnrollmentQuery : IRequest<IEnumerable<EnrollmentDTO>>;

    public class GetAllEnrollmentQueryHandler(IGeneralRepository<Entities.Enrollment> _repository) : IRequestHandler<GetAllEnrollmentQuery, IEnumerable<EnrollmentDTO>>
    {

        public async Task<IEnumerable<EnrollmentDTO>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var Enrollments =await _repository.GetAll();
            var EnrollmentDTOs = Enrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                Status = e.Status.ToString()

            });
            return EnrollmentDTOs;
           
        }
    }


}
