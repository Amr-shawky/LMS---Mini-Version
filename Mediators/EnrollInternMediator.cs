using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;

namespace LMS___Mini_Version.Mediators
{

    public class EnrollInternMediator
    {
        private readonly IInternService _internService;
        private readonly ITrackService _trackService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternMediator(
            IInternService internService,
            ITrackService trackService,
            IEnrollmentService enrollmentService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _internService = internService;
            _trackService = trackService;
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<EnrollmentResultDto> ExecuteAsync(CreateEnrollmentDto dto)
        {

            var intern = await _internService.GetByIdAsync(dto.InternId).ConfigureAwait(false);
            if (intern == null)
            {
                return EnrollmentResultDto.Fail($"Intern with ID {dto.InternId} was not found.");
            }

            var track = await _trackService.GetByIdAsync(dto.TrackId).ConfigureAwait(false);
            if (track == null)
            {
                return EnrollmentResultDto.Fail($"Track with ID {dto.TrackId} was not found.");
            }
            if (!track.IsActive)
            {
                return EnrollmentResultDto.Fail($"Track '{track.Name}' is not currently active.");
            }

            var hasCapacity = await _trackService.CheckCapacityAsync(dto.TrackId).ConfigureAwait(false);
            if (!hasCapacity)
            {
                return EnrollmentResultDto.Fail($"Track '{track.Name}' has reached its maximum capacity.");
            }

            var enrollment = await _enrollmentService.CreateEnrollmentAsync(dto).ConfigureAwait(false);

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            PaymentDto? payment = null;
            if (track.Fees > 0)
            {
                payment = await _paymentService.CreatePaymentAsync(new PaymentDto
                {
                    EnrollmentId = enrollment.Id,
                    Amount = track.Fees,
                    Method = PaymentMethod.Cash,
                    Status = PaymentStatus.Pending
                }).ConfigureAwait(false);
            }

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return EnrollmentResultDto.Succeed(new EnrollmentDto
            {
                Id = enrollment.Id,
                InternId = enrollment.InternId,
                InternName = intern.FullName,
                TrackId = enrollment.TrackId,
                TrackName = track.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status
            }, payment);
        }
    }

    public class EnrollmentResultDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public EnrollmentDto? Enrollment { get; set; }
        public PaymentDto? Payment { get; set; }

        public static EnrollmentResultDto Fail(string message) => new()
        {
            IsSuccess = false,
            ErrorMessage = message
        };

        public static EnrollmentResultDto Succeed(EnrollmentDto enrollment, PaymentDto? payment) => new()
        {
            IsSuccess = true,
            Enrollment = enrollment,
            Payment = payment
        };
    }
}
