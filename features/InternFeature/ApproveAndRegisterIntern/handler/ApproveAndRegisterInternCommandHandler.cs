using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.handler
{
    public class ApproveAndRegisterInternCommandHandler : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public ApproveAndRegisterInternCommandHandler(IGeneralRepository<Intern> repository,IUnitOfWork unitOfWork, IMediator mediator  )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();

            if (request.InternId <= 0)
            {
                errors.Add("InternId",new[] { "Intern ID must be greater than zero." });
            }

            if (!Enum.IsDefined(typeof(PaymentMethod), request.Method))
            {
                errors.Add("PaymentMethod",new[] { "Invalid payment method." });
            }

            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }
            var internData = await _repository.GetAll()
                .Where(i => i.Id == request.InternId)
                .Select(i => new
                {
                    Intern = i,
                    Track = i.Track,
                    HasDuplicateEnrollment = i.Enrollments.Any(e =>e.TrackId == i.TrackId &&(e.Status == EnrollmentStatus.Pending ||e.Status == EnrollmentStatus.Active))
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (internData is null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern not found." } } });
            }
            if(internData.Intern.Status != "Pending")
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern is not Pending." } } });
            }
            if(internData.Track== null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]> { { "Track", new string[] { "Track not found." } } });
            }
            if (!internData.Track.IsActive)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Track", new string[] { "Track is not active." } } });
            }
            if(internData.HasDuplicateEnrollment)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Enrollment", new string[] { "Intern has a duplicate enrollment for this track." } } });
            }

            var availabilityResponse = await _mediator.Send(new GetTrackAvailabilityQuery(internData.Intern.TrackId),cancellationToken);
            if (!availabilityResponse.Success)
            {
                return RequestResponse<int>.Fail(availabilityResponse.StatusCode,availabilityResponse.Errors);
            }

            if (availabilityResponse.Data == null || availabilityResponse.Data.AvailableSeats <= 0)
            {
                return RequestResponse<int>.Fail(409,new Dictionary<string, string[]>{ { "Track", new[] { "No available seats in this track." } } });
            }
            var now = DateTime.UtcNow;

            var enrollment = new Enrollment
            {
                TrackId = internData.Intern.TrackId,
                EnrollmentDate = now,
                Status = EnrollmentStatus.Active,
                Payment = new Payment
                {
                    Amount = internData.Track.Fees,
                    PaymentDate = now,
                    Method = request.Method,
                    Status = PaymentStatus.Completed
                }
            };
            internData.Intern.Status = "Accepted";
            internData.Intern.Enrollments.Add(enrollment);

            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(enrollment.Id);
        }
    }
}
