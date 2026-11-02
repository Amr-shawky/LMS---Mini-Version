using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.DTOs;
using LMS___Mini_Version.Features.Intern.GetInternById.Dtos;
using LMS___Mini_Version.Features.Track.GetTrackById.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Queries
{
    public record GetEnrollmentByIdQuery(int id) : IRequest<GetEnrollmentByIdDto>;

    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, GetEnrollmentByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEnrollmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetEnrollmentByIdDto> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().GetByIdAsync(request.id);
            if(enrollment == null) { return null; }
            var enrollmentDto = new GetEnrollmentByIdDto()
            {
                Id = enrollment.Id,
                PaymentStatus = enrollment.PaymentStatus,
                InternDto = new GetInternDto()
                {
                    TrackId = enrollment.TrackId,
                    TrackName = enrollment.Track.Name,
                    FullName = enrollment.Intern.FullName,
                    Id = enrollment.InternId
                },
                TrackId =enrollment.TrackId,
                TrackDto = new GetTrackDto()
                {
                    Fees = enrollment.Track.Fees,
                    IsActive = enrollment.Track.IsActive,
                    Name = enrollment.Track.Name
                }
            };
            return enrollmentDto;
        }
    }
}
