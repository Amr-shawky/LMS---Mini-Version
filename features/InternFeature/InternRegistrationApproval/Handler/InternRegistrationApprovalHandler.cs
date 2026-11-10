using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.Query;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.Handler
{
    public class InternRegistrationApprovalHandler(IUnitOfWork unitOfWork) : IRequestHandler<InternRegistrationApprovalQuery, RequestResponse>
    {


        public async Task<RequestResponse> Handle(InternRegistrationApprovalQuery request, CancellationToken cancellationToken)
        {
            var InternRepo = unitOfWork.GetRepository<Intern>();
            var intern = await InternRepo.GetByIdAsync(request.InternId);
            //intern Check
            if (intern == null)
                return RequestResponse.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["InternId"] = [$"Intern with Id {request.InternId} is Not Found"] });
            if (intern.Status.ToString() == InternStatus.Pending.ToString())
            {
                //track check
                var track = unitOfWork.GetRepository<Track>().GetAll()
                        .Where(w => w.Id == intern.TrackId)
                        .FirstOrDefault();
                if (track is null)
                    return RequestResponse.Fail(StatusCodes.Status404NotFound, new Dictionary<string, string[]> { ["TrackId"] = [$"Track with Id {intern.TrackId} is Not Found"] });
                if (!track.IsActive)
                    return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["TrackNotActive"] = [$"Track with Id {track.Id} is Not Active"] });
                //enrollment Check
                var enrollmentRepo = unitOfWork.GetRepository<Enrollment>();

                var alreadyEnrolled = enrollmentRepo.GetAll().Any(e =>
               e.InternId == intern.Id &&
               e.TrackId == track.Id &&
               (e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active));
                if (alreadyEnrolled)
                    return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["Enrollment"] = ["Intern already has a Pending or Active enrollment in this track"] });

                //Seats check

                var reservedSeats = enrollmentRepo.GetAll().Count(e => e.TrackId == track.Id && (e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active));
                if (reservedSeats >= track.MaxCapacity)
                    return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["Track"] = ["No available seats in this track"] });
                //Payment Check

                PaymentMethod method;

                switch (request.InternRegistrationApprovalDTO.PaymentMethod?.ToLower())
                {
                    case "cash":
                        method = PaymentMethod.Cash;
                        break;
                    case "creditcard":
                        method = PaymentMethod.CreditCard;
                        break;
                    case "banktransfer":
                        method = PaymentMethod.BankTransfer;
                        break;
                    case "online":
                        method = PaymentMethod.Online;
                        break;
                    default:
                        return RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["PaymentMethod"] = ["Invalid payment method"] });
                }

                intern.Status = "Accepted";

                var enrollment = new Enrollment
                {
                    InternId = intern.Id,
                    TrackId = track.Id,
                    Status = EnrollmentStatus.Active,
                    EnrollmentDate = DateTime.UtcNow,
                    Payment = new Payment
                    {
                        Amount = track.Fees,
                        Method = method,
                        Status = PaymentStatus.Completed,
                        PaymentDate = DateTime.UtcNow
                    }
                };

                enrollmentRepo.Add(enrollment);

                await unitOfWork.SaveChangesAsync();
                return RequestResponse.OK();
            }
            return
                RequestResponse.Fail(StatusCodes.Status400BadRequest, new Dictionary<string, string[]> { ["InternStatus"] = [$"Intern Status is Active "] });
            //return RequestResponse.Fail();
            //• Use the track already assigned to the intern; it must exist and be active.
            //• The track must have an available seat, using the availability rules above.
            //• Do not create another Pending or Active enrollment for the same intern and track.
            //• Record the enrollment and payment dates.Return the created enrollment ID.
            //• If the operation cannot finish successfully, none of these business changes may be left partially saved.
        }
    }
}
