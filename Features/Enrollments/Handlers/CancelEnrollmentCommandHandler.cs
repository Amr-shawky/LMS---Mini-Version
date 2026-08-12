using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo, IUnitOfWork unitOfWork)
        {
            _enrollmentRepo = enrollmentRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
        {
            //GetEnrolllment
            var enrollment = await _enrollmentRepo.GetTable()
                                                .Where(enrollment => enrollment.Id == request.EnrollmentId)
                                                .Include(e => e.Payment)
                                                .FirstOrDefaultAsync(cancellationToken);
            //refund
            if (enrollment is null || enrollment.Status == EnrollmentStatus.Cancelled)
                return Unit.Value;
            if (enrollment.Payment is not null)
                enrollment.Payment.Status = PaymentStatus.Refunded;
            //cancel
            enrollment.Status = EnrollmentStatus.Cancelled;
            //update
            _enrollmentRepo.Update(enrollment);
            //save
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;

        }
    }

}
