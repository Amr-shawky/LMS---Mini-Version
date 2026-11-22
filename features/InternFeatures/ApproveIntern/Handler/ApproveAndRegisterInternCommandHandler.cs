using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.ApproveInternRegistration.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using LMS___Mini_Version.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;

namespace LMS___Mini_Version.features.InternFeatures.ApproveIntern.Handler
{
    public class ApproveAndRegisterInternCommandHandler(IGeneralRepository<Intern> internRepo,
        IUnitOfWork unitOfWork) : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> internRepo = internRepo;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
        {
           var data = await internRepo.GetAll().Where(x=>x.Id == request.InternId).Select(x=> new 
           {
               Intern = x,
               Track = x.Track,
               AlreadyEnrolled = x.Enrollments.Any(e =>
             e.TrackId == x.TrackId &&
             (e.Status == EnrollmentStatus.Pending ||
              e.Status == EnrollmentStatus.Active)),
               ReservedSeats = x.Track.Enrollments.Count(e =>
                   e.Status == EnrollmentStatus.Pending ||
                   e.Status == EnrollmentStatus.Active)
           }).FirstOrDefaultAsync();

           

            if (data?.Intern == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern not found" }
                });
            }
            if(data.Intern.Status!="Pending")
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern is not in pending status" }
                });
            }


            if (data.Track == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(data.Track)] = new[] { "Track not found" }
                });
            }

            if (!data.Track.IsActive)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(data.Track.Id)] = new[] { "Track is not active" }
                });
            }
            if (data.AlreadyEnrolled)
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern is already enrolled in this track" }
                });
            }
            if (data.ReservedSeats >= data.Track.MaxCapacity)
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] {"Track has reached its maximum capacity"}
                });
            }

            if (!Enum.IsDefined(request.PaymentMethod))
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.PaymentMethod)] = new[] { "Invalid payment method" }
                });
            }

            data.Intern.Status = "Accepted";
            var enrollment = new Enrollment
            {
                InternId = data.Intern.Id,
                TrackId = data.Intern.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active,
                Payment= new Payment
                {
                    Method = request.PaymentMethod,
                    Amount = data.Intern.Track.Fees,
                    PaymentDate = DateTime.UtcNow,
                    Status = PaymentStatus.Completed
                }
            };

            data.Intern.Enrollments.Add(enrollment);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(enrollment.Id);
        }
    }
}
