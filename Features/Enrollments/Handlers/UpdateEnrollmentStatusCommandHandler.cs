using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IUnitOfWork _uow;

        public UpdateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo, IUnitOfWork Uow)
        {
            _enrollmentRepo = enrollmentRepo;
            _uow = Uow;
        }
        public async Task<Unit> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentId);
            if (enrollment is null)
                throw new KeyNotFoundException();
            enrollment.Status = request.Status;
            _enrollmentRepo.Update(enrollment);
            await _uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
