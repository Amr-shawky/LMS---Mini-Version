using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Handlers
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var info = await _repository.GetAll()
                .Where(e => e.Id == request.id)
                .Select(e => new
                {
                    e.Status,
                    PaymentCompleted = e.Payment != null && e.Payment.Status == PaymentStatus.Completed
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (info == null)
                return RequestResponse.Fail(404, "EnrollmentId", "Enrollment Not Found");

            if (info.Status != EnrollmentStatus.Active)
                return RequestResponse.Fail(400, "EnrollmentId", "Only an Active enrollment can be completed");

            if (!info.PaymentCompleted)
                return RequestResponse.Fail(400, "Payment", "Enrollment must have a Completed payment");

            var enrollment = await _repository.GetByIdAsync(request.id);
            enrollment!.Status = EnrollmentStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
