
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegister.Commands;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegister.Handler
{
    public class ApproveAndRegisterCommandHandler
        : IRequestHandler<ApproveAndRegisterCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISender sender;

        public ApproveAndRegisterCommandHandler(
            IGeneralRepository<Intern> repository,
            IUnitOfWork unitOfWork,
            ISender sender)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.sender = sender;
        }

        public async Task<RequestResponse<int>> Handle(
            ApproveAndRegisterCommand request,
            CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(request.Method))
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["PaymentMethod"] = new[]
                        {
                            "Invalid payment method."
                        }
                    });
            }
            var internData = await repository.GetAll()
                .Where(i => i.Id == request.InternId)
                .Select(i => new
                {
                    Intern = i,

                    HasEnrollment = i.Enrollments.Any(e =>
                        e.TrackId == i.TrackId &&
                        (e.Status == EnrollmentStatus.Pending ||
                         e.Status == EnrollmentStatus.Active))
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (internData is null)
            {
                return RequestResponse<int>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["InternId"] = new[]
                        {
                            "Intern not found."
                        }
                    });
            }

            if (internData.Intern.Status != "Pending")
            {
                return RequestResponse<int>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["InternStatus"] = new[]
                        {
                            "Only Pending interns can be approved."
                        }
                    });
            }
            var trackResult = await sender.Send(
                new GetTrackAvailabilityQuery(
                    internData.Intern.TrackId),
                cancellationToken);

            if (!trackResult.Success || trackResult.Data is null)
            {
                return RequestResponse<int>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[]
                        {
                            "Track not found."
                        }
                    });
            }

            if (!trackResult.Data.IsActive)
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["Track"] = new[]
                        {
                            "Selected track is not active."
                        }
                    });
            }
            if (trackResult.Data.AvailableSeats <= 0)
            {
                return RequestResponse<int>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["Capacity"] = new[]
                        {
                            "Track has no available seats."
                        }
                    });
            }
            if (internData.HasEnrollment)
            {
                return RequestResponse<int>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["Enrollment"] = new[]
                        {
                            "Intern already has a Pending or Active enrollment."
                        }
                    });
            }
            var trackFees = await repository.GetAll()
                .Where(i => i.Id == request.InternId)
                .Select(i => (decimal?)i.Track.Fees)
                .FirstOrDefaultAsync(cancellationToken);

            if (trackFees is null)
            {
                return RequestResponse<int>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[]
                        {
                            "Assigned track not found."
                        }
                    });
            }
            internData.Intern.Status = "Accepted";

            var now = DateTime.UtcNow;

            var enrollment = new Enrollment
            {
                InternId = internData.Intern.Id,
                TrackId = internData.Intern.TrackId,
                EnrollmentDate = now,
                Status = EnrollmentStatus.Active,

                Payment = new Payment
                {
                    Amount = trackFees.Value,
                    PaymentDate = now,
                    Status = PaymentStatus.Completed,
                    Method = request.Method
                }
            };
            internData.Intern.Enrollments.Add(enrollment);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(enrollment.Id, 201);
        }
    }
}
