using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Handler
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository , IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Get Enrollment with its Payment
            var enrollment = await _enrollmentRepository.GetAll()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(
                    x => x.Id == request.EnrollmentId,
                    cancellationToken);

            if(enrollment is null)
            {
                return RequestResponse.Fail
                 (
                        404,
                        new Dictionary<string, string[]>
                        {
                            { "EnrollmentId", new[] { "Enrollment not found." } }
                        }
                 );
            }

            // 3. Check Enrollment Status
            if (enrollment.Status != EnrollmentStatus.Active)
            {
                return RequestResponse.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                    { "Status", new[] { "Only active enrollments can be completed." } }
                    });
            }

            // 4. Check Payment
            if (enrollment.Payment is null ||
                enrollment.Payment.Status != PaymentStatus.Completed)
            {
                return RequestResponse.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                    { "Payment", new[] { "Enrollment requires a completed payment." } }
                    });
            }

            // 5. Update Enrollment Status only
            enrollment.Status = EnrollmentStatus.Completed;

            // 6. Save changes
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
