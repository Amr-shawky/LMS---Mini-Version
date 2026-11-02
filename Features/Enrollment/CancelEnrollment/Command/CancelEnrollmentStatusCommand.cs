using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Command
{
    public record CancelEnrollmentStatusCommand(int enrollmentId) : IRequest<bool>;
    public class CancelEnrollmentStatusCommandHandler : IRequestHandler<CancelEnrollmentStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CancelEnrollmentStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CancelEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
           var enrollment = await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().GetByIdAsync(request.enrollmentId);
            if (enrollment == null) { return false; }
            enrollment.EnrollmentStatus = "Cancelled";
             _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().SaveInclude(enrollment, nameof(LMS___Mini_Version.Domain.Entities.Enrollment.EnrollmentStatus));
            return true;
        }
    }
}
