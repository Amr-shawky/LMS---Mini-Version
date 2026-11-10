using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteTraining
{
    public class CompleteTrainingHandler : IRequestHandler<CompleteTrainingCommand, RequestResponse>
    {
        readonly private IGeneralRepository<Enrollment> _enrollmentRepository;
        readonly private IUnitOfWork _unitOfWork;
        public CompleteTrainingHandler(IGeneralRepository<Enrollment> enrollmentRepository, IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(CompleteTrainingCommand request, CancellationToken cancellationToken)
        {
            //var enrollment = _enrollmentRepository.GetAll().Select(e => new
            //{
            //    e.Id,
            //    e.Status,
            //    PaymentStatus = e.Payment.Status,

            //}).FirstOrDefault(e => e.Id == request.enrollmentId);
            var result = _enrollmentRepository.GetAll().Where(e => e.Id == request.enrollmentId).Select(e => new
            {
                Enrollment = e,
                PaymentStatus = e.Payment.Status,
            }).FirstOrDefault();

            if (result == null)
                return RequestResponse.Fail(404, ErrorCode.EnrollmentNotFound);

            Enrollment enrollment = result.Enrollment;
            if (enrollment.Status == EnrollmentStatus.Completed)
                return RequestResponse.Fail(400, ErrorCode.EnrollmentAlreadyCompleted);
            if (enrollment.Status == EnrollmentStatus.Cancelled)
                return RequestResponse.Fail(400,ErrorCode.EnrollmentIsCancelled);
            if(enrollment.Status != EnrollmentStatus.Active)
                return RequestResponse.Fail(400, ErrorCode.EnrollmentIsNotActive);
            if(result.PaymentStatus != PaymentStatus.Completed)
                return RequestResponse.Fail(400, ErrorCode.EnrollmentPaymentCompleted);

            //Enrollment enrollmentToUpdate = new Enrollment
            //{
            //    Id = enrollment.Id,
            //    Status = EnrollmentStatus.Completed
            //};
            enrollment.Status = EnrollmentStatus.Completed;
         
            _unitOfWork.SaveChangesAsync();
            //  _unitOfWork.SaveIncludesAsync(enrollmentToUpdate, nameof(Enrollment.Status));

            return RequestResponse.OK();
        }
    }
}
