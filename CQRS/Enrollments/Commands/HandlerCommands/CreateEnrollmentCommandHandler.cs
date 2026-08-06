using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.HandlerCommands;

public class CreateEnrollmentCommandHandler(IGeneralRepository<Enrollment> _enrollmentRepo,IUnitOfWork _unitOfWork)
    : IRequestHandler<CreateEnrollmentCommand, EnrollmentDto>
{   

    public async Task<EnrollmentDto> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Enrollment 
        {
            InternId = request.InternId,
            TrackId = request.TrackId,
            EnrollmentDate = DateTime.UtcNow,
            Status = EnrollmentStatus.Pending
        };
        _enrollmentRepo.Add(entity);
        await _unitOfWork.CompleteAsync();

        var dto = entity.ToDto();

        return dto;
    }

}
