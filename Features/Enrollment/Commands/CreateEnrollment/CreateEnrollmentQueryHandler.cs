using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Commands.CreateEnrollment
{
    public class CreateEnrollmentQueryHandler : IRequestHandler<CreateEnrollmentQuery, EnrollmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEnrollmentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EnrollmentDto> Handle(CreateEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var entity = new  Domain.Entities.Enrollment
            {
                InternId = request.dto.InternId,
                TrackId = request.dto.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending
            };

            _unitOfWork.Enrollments.Add(entity);
            return entity.ToDto();
        }
    }
}
