using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record CompleteTrainingEnrollmentCommand(int enrollmentID) : IRequest<RequestResponse>;
    
    
    
    public class CompleteTrainingEnrollmentCommandHandler : IRequestHandler<CompleteTrainingEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IGeneralRepository<Payment> _paymenttRepo;

        public CompleteTrainingEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo,IGeneralRepository<Payment> paymentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _paymenttRepo = paymentRepo;
        }

        public async Task<RequestResponse> Handle(CompleteTrainingEnrollmentCommand command,CancellationToken cancellationToken)
        {
            var enrollments = await _enrollmentRepo.GetTable().Where(e=>e.Id==command.enrollmentID)
                .Where(e=>e.Status==EnrollmentStatus.Active).FirstOrDefaultAsync(cancellationToken);

            if (enrollments == null)
                return null;


            var payment = await _paymenttRepo.GetTable()
                .Where(p=>p.EnrollmentId == command.enrollmentID)
                .Where(p=>p.Status==PaymentStatus.Completed).FirstOrDefaultAsync(cancellationToken);
            
            if (payment is null)
                return RequestResponse.Fail();

            enrollments.Status = EnrollmentStatus.Completed;

            _enrollmentRepo.Update(enrollments);
            
            return RequestResponse.OK();
        }
    }
}
