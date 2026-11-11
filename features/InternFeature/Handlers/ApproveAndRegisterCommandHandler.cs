using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Queries;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class ApproveAndRegisterCommandHandler : IRequestHandler<ApproveAndRegisterCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ApproveAndRegisterCommandHandler(IGeneralRepository<Intern> repository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
             _repository = repository;
             _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

     

       public async  Task<RequestResponse<bool>>  Handle(ApproveAndRegisterCommand request, CancellationToken cancellationToken)
        {
            var internData = await _repository.GetAll()
               .Where(i => i.Id == request.internId)
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
                Dictionary<string, string[]> Errors = new Dictionary<string, string[]>
                {
                    ["InternId"] = new[]
                        {
                            "Intern not found."
                        }
                };
                return RequestResponse<bool>.Fail(404, Errors);
            }

            if (internData.Intern.Status != "Pending")
            {
                Dictionary<string, string[]> Errors = new Dictionary<string, string[]>
                {
                    ["InternStatus"] = new[]
                        {
                            "Only Pending interns can be approved."
                        }
                };  
                return RequestResponse<bool>.Fail(
                    409,
                   Errors);
            }
            var trackResult = await _mediator.Send(
                new CheckTrackAvaiabilityQuery(
                    internData.Intern.TrackId),
                cancellationToken);

            if (!trackResult.Success || trackResult.Data is null)
            {
                Dictionary<string, string[]> Errors = new Dictionary<string, string[]>
                {
                    ["TrackId"] = new[]
                        {
                            "Track not found."
                        }
                };
                return RequestResponse<bool>.Fail(
                    404,
                    Errors);
            }

            if (!trackResult.Data.IsActive)
            {
                Dictionary<string, string[]> Errors = new Dictionary<string, string[]>
                {
                    ["Track"] = new[]
                        {
                            "Selected track is not active."
                        }
                };
                return RequestResponse<bool>.Fail(
                    400,
                   Errors);
            }
            if (trackResult.Data.AvailableSeats <= 0)
            {
                Dictionary<string, string[]> Errors = new Dictionary<string, string[]>
                {
                    ["Capacity"] = new[]
                        {
                            "Track has no available seats."
                        }
                };
                return RequestResponse<bool>.Fail(
                    409,
                    Errors);
            }
            if (internData.HasEnrollment)
            {

                return RequestResponse<bool>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["Enrollment"] = new[]
                        {
                            "Intern already has a Pending or Active enrollment."
                        }
                    });
            }
            var trackFees = await _repository.GetAll()
                .Where(i => i.Id == request.internId)
                .Select(i => (decimal?)i.Track.Fees)
                .FirstOrDefaultAsync(cancellationToken);

            if (trackFees is null)
            {
                return RequestResponse<bool>.Fail(
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
                    Method = request.PaymentMethod
                }
            };
            internData.Intern.Enrollments.Add(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<bool>.OK(true, 201);
        }
    }
}
