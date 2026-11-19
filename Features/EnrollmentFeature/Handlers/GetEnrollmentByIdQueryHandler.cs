using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.EnrollmentFeature.Dtos;
using LMS___Mini_Version.Features.EnrollmentFeature.Quries;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Handlers
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, RequestResponse<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;

        }
        public async Task<RequestResponse<EnrollmentDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var ennrollment = await _repository.
                GetByIdAsync(request.ID);


            var dto = new EnrollmentDto
            {
                InternId = ennrollment.InternId,
                TrackId = ennrollment.TrackId,
                Status = ennrollment.Status,
                EnrollmentDate = ennrollment.EnrollmentDate
            };

            return RequestResponse<EnrollmentDto>.OK(200,dto);

        }
    }
}
