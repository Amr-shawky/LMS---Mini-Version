using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository , IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(request.EnrollmentId);

            if (enrollment == null)
                throw new KeyNotFoundException($"EnrollmentId {request.EnrollmentId} Not Found");

            enrollment.TrackId = request.NewTrackId;

            _enrollmentRepository.Update(enrollment);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
