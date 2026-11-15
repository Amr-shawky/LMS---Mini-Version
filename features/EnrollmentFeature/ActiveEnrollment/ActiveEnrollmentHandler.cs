using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.ApproveRegisterIntern;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.ActiveEnrollment
{
    public class ActiveEnrollmentHandler : IRequestHandler<ActiveEnrollmentCommand, RequestResponse<int>>
    {
        IGeneralRepository<Enrollment> _enrollmentRepo;
        public ActiveEnrollmentHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        public async Task<RequestResponse<int>> Handle(ActiveEnrollmentCommand request, CancellationToken cancellationToken)
        {
            Enrollment? enrollment = _enrollmentRepo.GetAll().Where(e => e.InternId == request.internId && e.TrackId == request.trackId).FirstOrDefault();
            if (enrollment is null)
                return RequestResponse<int>.Fail(400, errorCode: ErrorCode.InternHasNoEnrollment);
            if (enrollment.Status != EnrollmentStatus.Pending)
                return RequestResponse<int>.Fail(400, errorCode: ErrorCode.InternHasNoPendingEnrollment);
            enrollment.Status = EnrollmentStatus.Active;
            enrollment.EnrollmentDate = DateTime.Now;
            _enrollmentRepo.Update(enrollment);
            return RequestResponse<int>.OK(enrollment.Id);

        }
    }

}
