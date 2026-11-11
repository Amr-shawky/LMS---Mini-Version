
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Handler
{
    public class CompleteEnrollmentCommandHandler
        : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> repository;
        private readonly IUnitOfWork unitOfWork;

        public CompleteEnrollmentCommandHandler(
            IGeneralRepository<Enrollment> repository,
            IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(
            CompleteEnrollmentCommand request,
            CancellationToken cancellationToken)
        {
            var enrollment = await repository.GetAll()
                .Where(e => e.Id == request.EnrollmentId)
                .Select(e => new
                {
                    Enrollment = e,
                    PaymentStatus = e.Payment != null
                        ? (PaymentStatus?)e.Payment.Status
                        : null
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (enrollment is null)
            {
                return RequestResponse.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["Enrollment"] = new[]
                        {
                            "Enrollment not found."
                        }
                    });
            }
            if (enrollment.Enrollment.Status != EnrollmentStatus.Active)
            {
                return RequestResponse.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["EnrollmentStatus"] = new[]
                        {
                            "Only Active enrollments can be completed."
                        }
                    });
            }
            if (enrollment.PaymentStatus is null)
            {
                return RequestResponse.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["Payment"] = new[]
                        {
                            "Enrollment has no associated payment."
                        }
                    });
            }
            if (enrollment.PaymentStatus != PaymentStatus.Completed)
            {
                return RequestResponse.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["PaymentStatus"] = new[]
                        {
                            "Payment must be Completed before completing enrollment."
                        }
                    });
            }
            enrollment.Enrollment.Status = EnrollmentStatus.Completed;
            await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK(200);
        }
    }
}
