using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.handler
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository, IUnitOfWork unitOfWork   )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            if (request.EnrollmentId <= 0)
            {
                return RequestResponse.Fail(400,new Dictionary<string, string[]>{{ "EnrollmentId", new[] {"Enrollment ID must be greater than zero"}}});
            }
            var enrollment = await _repository.GetByIdAsync(request.EnrollmentId);
            if(enrollment == null)
            {
                return RequestResponse.Fail(404, new Dictionary<string, string[]> {{ "Enrollment", new[] { "Enrollment not found" }}});
            }
            if(enrollment.Status != EnrollmentStatus.Active)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]> { { "Enrollment", new[] { "Enrollment is not active,Only active enrollments can be completed" } }});
            }
            var paymentStatus = await _repository.GetAll()
                    .Where(e => e.Id == request.EnrollmentId)
                    .Select(e => e.Payment == null? (PaymentStatus?)null: e.Payment.Status)
                    .FirstOrDefaultAsync(cancellationToken);
            if(paymentStatus == null || paymentStatus != PaymentStatus.Completed)
            {
                return RequestResponse.Fail(409, new Dictionary<string, string[]> { { "Payment", new[] { "Payment is not completed,Only enrollments with completed payments can be completed" }}});
            }

            enrollment.Status = EnrollmentStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK(200);
        }
    }
}
