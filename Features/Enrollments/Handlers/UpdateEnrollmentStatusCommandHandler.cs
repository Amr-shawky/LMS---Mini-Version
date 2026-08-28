using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class UpdateEnrollmentStatusCommandHandler
(IGeneralRepository<Enrollment> _enrollmentrepo,
    IUnitOfWork _unitOfWork) : IRequestHandler<UpdateEnrollmentStatusCommand, Unit>
{


    public async Task<Unit> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentrepo.GetTable()
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

        if (enrollment == null)
            throw new Exception("Enrollment not found");

        enrollment.Status = request.NewStatus;

        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}