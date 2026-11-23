using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CreateEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CreateEnrollment.handler
{
    public class CreateEnrollmentCommandHandler: IRequestHandler<CreateEnrollmentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(CreateEnrollmentCommand request,CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (request.InternId <= 0)
            {
                errors.Add("InternId",new[] { "Intern ID must be greater than zero." });
            }
            if (request.TrackId <= 0)
            {
                errors.Add("TrackId",new[] { "Track ID must be greater than zero." });
            }
            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }

            var exists = await _repository.GetAll()
                .AnyAsync(e =>
                    e.InternId == request.InternId &&
                    e.TrackId == request.TrackId &&
                    (e.Status == EnrollmentStatus.Pending ||
                     e.Status == EnrollmentStatus.Active),cancellationToken);

            if (exists)
            {
                return RequestResponse<int>.Fail(409,new Dictionary<string, string[]>{{"Enrollment",new[]{"Intern already has an active or pending enrollment for this track." }}});
            }

            var enrollment = new Enrollment
            {
                InternId = request.InternId,
                TrackId = request.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            _repository.Add(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(enrollment.Id, 201);
        }
    }
}
