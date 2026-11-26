using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record ApproveAndCompleteInternRegistrationCommand(int internId, string paymentMethod) : IRequest<RequestResponse>;

    public class ApproveAndCompleteInternRegistrationCommandHandler : IRequestHandler<ApproveAndCompleteInternRegistrationCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Intern> _internRepo;
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IGeneralRepository<Payment> _paymentRepo;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public ApproveAndCompleteInternRegistrationCommandHandler(IGeneralRepository<Intern> internRepo, IGeneralRepository<Track> trackRepo, IGeneralRepository<Payment> paymentRepo, IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _internRepo = internRepo;
            _trackRepo = trackRepo;
            _paymentRepo = paymentRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse> Handle(ApproveAndCompleteInternRegistrationCommand command, CancellationToken cancellationToken)
        {
            PaymentMethod paymentrecieved;
            if (command.paymentMethod.ToLower() == "cash")
                paymentrecieved = PaymentMethod.Cash;
            else if (command.paymentMethod.ToLower() == "creditcard")
                paymentrecieved = PaymentMethod.CreditCard;
            else if (command.paymentMethod.ToLower() == "online")
                paymentrecieved = PaymentMethod.Online;
            else if (command.paymentMethod.ToLower() == "banktransfer")
                paymentrecieved = PaymentMethod.BankTransfer;
            else
                return RequestResponse.Fail(400,null,"PaymentMethod entered is Invalid");

            var intern = _internRepo.GetById(command.internId);
            if (intern == null)
                return RequestResponse.Fail(404,null,"Intern Id is Invalid");

            var internStatus = _internRepo.GetById(command.internId).Status;
            if (internStatus != "Pending")
                return RequestResponse.Fail(409,null,"Intern Status Must be Pending");

            var track = await _trackRepo.GetTable().FirstOrDefaultAsync(t=>t.Id==intern.TrackId,cancellationToken);
            if (track == null)
                return RequestResponse.Fail(404,null,"Track doesn't Exist");

            if(!track.IsActive)
                return RequestResponse.Fail(409,null,"Track must be Active");

            var OccupiedSeats = await _enrollmentRepo.GetTable().CountAsync(e=>
            e.TrackId == track.Id &&
            e.Status==EnrollmentStatus.Active,cancellationToken);

            if (track.MaxCapacity<= OccupiedSeats)
                return RequestResponse.Fail(409,null,"There is No available Seats in this Track");
            var enrollment = _enrollmentRepo.GetTable().FirstOrDefaultAsync(e => e.InternId == command.internId && e.TrackId==track.Id);

            if (enrollment is null)
            {
                var NewEnrollment = new Enrollment
                {
                    InternId = command.internId,
                    Intern = intern,
                    TrackId = track.Id,
                    Track = track,
                    EnrollmentDate = DateTime.Now,
                    Status = EnrollmentStatus.Active
                };

                _enrollmentRepo.Add(NewEnrollment);

                var NewPayment = new Payment
                {
                    Enrollment = NewEnrollment,
                    Amount = track.Fees,
                    EnrollmentId = NewEnrollment.Id,
                    PaymentDate = DateTime.Now,
                    Method = paymentrecieved,
                    Status = PaymentStatus.Completed
                };
                _paymentRepo.Add(NewPayment);

            }
            else
            {
                return RequestResponse.Fail(409,null,"Enrollment exists already");
            }
            intern.Status = "Accepted";
            return RequestResponse.OK();
        }
    }
}