using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.CompleteEnrollment.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.CompleteEnrollment.Handler
{
    public class CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo,IUnitOfWork unitOfWork) : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> enrollmentRepo = enrollmentRepo;

        public IUnitOfWork UnitOfWork { get; } = unitOfWork;

        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await enrollmentRepo.GetAll().Where(x=>x.Id == request.EnrollmentId)
                .Select(x=> new
                {
                    Enrollment = x,
                    EnrollmentStatus = x.Status,
                    PaymentStatus =x.Payment ==null ? (PaymentStatus?)null : x.Payment.Status,
                }).FirstOrDefaultAsync();


            if (enrollment == null)
            {
                return RequestResponse.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment not found" }
                });
            }
            if (enrollment.EnrollmentStatus == EnrollmentStatus.Completed)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment is already completed" }
                });
            }
            if(enrollment.PaymentStatus == null || enrollment.PaymentStatus != PaymentStatus.Completed)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment payment is not completed" }
                });
            }
            if(enrollment.EnrollmentStatus!=EnrollmentStatus.Active)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment is not active" }
                });
            }
            enrollment.Enrollment.Status = EnrollmentStatus.Completed;
            await UnitOfWork.SaveChangesAsync();
            return RequestResponse.OK();

        }
    }
}
