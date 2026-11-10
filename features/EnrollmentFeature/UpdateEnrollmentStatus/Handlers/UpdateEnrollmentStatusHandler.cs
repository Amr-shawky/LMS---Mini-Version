using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollmentStatus.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollmentStatus.Handlers
{
    public class UpdateEnrollmentStatusHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateEnrollmentStatusCommand, RequestResponse>
    {
        public async Task<RequestResponse> Handle(
            UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
            var enrollmentId = request.enrollmentId;

            var enrollment = await unitOfWork.GetRepository<Enrollment>().GetAll()
                .Include(e => e.Payment)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken: cancellationToken);

            if (enrollment is null)
                return RequestResponse.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["EnrollmentId"] = [$"Enrollment with Id {enrollmentId} is not found"] });

            if (enrollment.Status != EnrollmentStatus.Active)
                return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["EnrollmentStatus"] = [$"Only an Active enrollment can be completed Current status is {enrollment.Status}"] });

            if (enrollment.Payment is null || enrollment.Payment.Status != PaymentStatus.Completed)
                return RequestResponse.Fail(StatusCodes.Status400BadRequest,
                    new Dictionary<string, string[]> { ["PaymentStatus"] = ["The enrollment must have a Completed payment before it can be completed"] });
            enrollment.Status = EnrollmentStatus.Completed;
            //await unitOfWork.SaveChangesAsync();
            return RequestResponse.OK();
        }
    }
}