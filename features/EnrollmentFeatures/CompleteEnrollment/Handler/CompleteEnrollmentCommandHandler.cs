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
            var enrollment = await enrollmentRepo.GetAll()
                .Select(x=> new Enrollment
                {
                    Id = x.Id,
                    Status = x.Status,
                    Payment = x.Payment
                }).FirstOrDefaultAsync(x => x.Id == request.EnrollmentId);


            if (enrollment == null)
            {
                return RequestResponse.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment not found" }
                });
            }
            if (enrollment.Status == EnrollmentStatus.Completed)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment is already completed" }
                });
            }
            if(enrollment.Payment == null || enrollment.Payment.Status != PaymentStatus.Completed)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment payment is not completed" }
                });
            }
            if(enrollment.Status!=EnrollmentStatus.Active)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.EnrollmentId)] = new[] { "Enrollment is not active" }
                });
            }
            enrollment.Status = EnrollmentStatus.Completed;
            await UnitOfWork.SaveChangesAsync();
            return RequestResponse.OK();

        }
    }
}
