using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.ApproveInternRegistration.Command;
using LMS___Mini_Version.features.shared;
using MediatR;
using LMS___Mini_Version.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.ApproveIntern.Handler
{
    public class ApproveAndRegisterInternCommandHandler(IGeneralRepository<Intern> internRepo,
        IGeneralRepository<Enrollment> enrollmentRepo,
        IGeneralRepository<Track> trackRepo,
        IUnitOfWork unitOfWork) : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> internRepo = internRepo;
        private readonly IGeneralRepository<Enrollment> enrollmentRepo = enrollmentRepo;
        private readonly IGeneralRepository<Track> trackRepo = trackRepo;

        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
        {
           var intern = await internRepo.GetAll().Where(x=>x.Id == request.InternId).FirstOrDefaultAsync();
           

            if (intern == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern not found" }
                });
            }
            if(intern.Status!="Pending")
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern is not in pending status" }
                });
            }

            var track = await trackRepo.GetAll().FirstOrDefaultAsync(x => x.Id == intern.TrackId);

            if (track == null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(intern.TrackId)] = new[] { "Track not found" }
                });
            }

            if (!track.IsActive)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(intern.TrackId)] = new[] { "Track is not active" }
                });
            }
            var alreadyEnrolled = await enrollmentRepo.GetAll().AnyAsync
                (x=>x.InternId==intern.Id && x.TrackId==intern.TrackId &&(x.Status == EnrollmentStatus.Pending || x.Status == EnrollmentStatus.Active));
            if(alreadyEnrolled)
            {
                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
                {
                    [nameof(request.InternId)] = new[] { "Intern is already enrolled in this track" }
                });
            }
            var enrollmentCount = await enrollmentRepo.GetAll().CountAsync(x => x.TrackId == intern.TrackId && (x.Status == EnrollmentStatus.Pending || x.Status == EnrollmentStatus.Active));
            if (enrollmentCount >= track.MaxCapacity)
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

            intern.Status = "Accepted";
            var enrollment = new Enrollment
            {
                InternId = intern.Id,
                TrackId = intern.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active,
                Payment= new Payment
                {
                    Method = request.PaymentMethod,
                    Amount = track.Fees,
                    PaymentDate = DateTime.UtcNow,
                    Status = PaymentStatus.Completed
                }
            };

            enrollmentRepo.Add(enrollment);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(enrollment.Id);


        }
    }
}
