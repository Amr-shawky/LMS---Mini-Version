using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Handlers
{
    public class CreateActiveEnrollmentCommandHandler : IRequestHandler<CreateActiveEnrollmentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateActiveEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(CreateActiveEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.GetAll()
                .Where(e => e.InternId == request.InternId)
                .Where(e => e.TrackId == request.TrackId)
                .AnyAsync(e => e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending);

            if (exists)
                return RequestResponse<int>.Fail(ErrorCode.InternHasActiveOrPendingEnrollments);

            var enrollment = new Enrollment
            {
                InternId = request.InternId,
                TrackId = request.TrackId,
                Status = EnrollmentStatus.Active,
                EnrollmentDate = DateTime.UtcNow,
            };

            _repository.Add(enrollment);

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse<int>.OK(enrollment.Id);

        }
    }
}
