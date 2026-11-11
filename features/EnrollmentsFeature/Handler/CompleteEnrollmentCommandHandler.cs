using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentsFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentsFeature.Handler
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _repository.GetAll().Where(x => x.Id == request.EnrollmentId).Select(x => new Enrollment()
            {
                Id = x.Id,
                Payment = x.Payment,
                Status = x.Status,
                InternId = x.InternId,
                Intern = x.Intern,
                EnrollmentDate = x.EnrollmentDate,
                TrackId = x.TrackId,
                Track = x.Track
            }).FirstOrDefaultAsync();

            if (enrollment is null)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement" , new[]{" Cannot Find Enrollement with that id"} }
                };
                return RequestResponse.Fail(404, erros);
            }
            if (enrollment.Payment == null || enrollment.Payment.Status == Domain.Enums.PaymentStatus.Failed || enrollment.Payment.Status == Domain.Enums.PaymentStatus.Refunded || enrollment.Payment.Status == Domain.Enums.PaymentStatus.Pending)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Payment" , new[]{" Cannot Complete Enrollement with Pending or Refunded or Failed Payment Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
            if (enrollment.Status != Domain.Enums.EnrollmentStatus.Active)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Status" , new[]{" Cannot Complete Enrollement with InActive Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
            if (enrollment.Status == Domain.Enums.EnrollmentStatus.Completed || enrollment.Status == Domain.Enums.EnrollmentStatus.Cancelled)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Status" , new[]{" Cannot Complete Enrollement with Completed or Cancelled Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
            enrollment.Status = Domain.Enums.EnrollmentStatus.Completed;
            _repository.Update(enrollment);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return RequestResponse.OK(200);
            }
            else
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement" , new[]{" Cannot Complete Enrollement"} }
                };
                return RequestResponse.Fail(400, erros);
            }
        }
    }
}
