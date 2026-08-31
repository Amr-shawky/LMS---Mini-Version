using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository , IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(request.EnrollmentID);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollement Not Found ");
            enrollment.TrackId = request.NewTrackID;
            _enrollmentRepository.Update(enrollment);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
