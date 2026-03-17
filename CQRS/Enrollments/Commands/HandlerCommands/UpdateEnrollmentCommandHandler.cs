using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.HandlerCommands
{
    public class UpdateEnrollmentCommandHandler(IUnitOfWork _unitOfWork,IMediator _mediator) : IRequestHandler<UpdateEnrollmentCommand, EnrollmentViewModel>
    {
        public async Task<EnrollmentViewModel> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments.GetTable().Include(e=>e.Intern).Include(e=>e.Track).FirstOrDefaultAsync(e => e.Id == request.Id);
            if (enrollment == null) 
                throw new Exception($"Enrollment with ID {request.Id} not found.");

            var intern = await _unitOfWork.Interns.GetByIdAsync(request.internId);
            if (intern == null)
            {
                throw new Exception($"Intern with ID {request.internId} not found.");
            }
            var track = await _unitOfWork.Tracks.GetByIdAsync(request.trackId);
            if (track == null)
            {
                throw new Exception($"Track with ID {request.trackId} not found.");
            }
            var capacity = await _mediator.Send(new GetEnrollmentCountByTrackQuery(request.trackId), cancellationToken);
            if (capacity >= track.MaxCapacity)
            {
                throw new Exception($"Track with ID {request.trackId} has reached its maximum capacity.");
            }
            var existingEnrollment = await _unitOfWork.Enrollments.GetTable().AnyAsync(e=>e.InternId == request.internId && e.TrackId == request.trackId && e.Id != request.Id);
            if (existingEnrollment)
            {
                throw new Exception($"Intern with ID {request.internId} is already enrolled in Track with ID {request.trackId}.");
            }
            enrollment.InternId = request.internId;
            enrollment.TrackId = request.trackId;

             //_unitOfWork.Enrollments.Update(enrollment);
             await _unitOfWork.CompleteAsync();
            return new EnrollmentViewModel
            {
                Id = enrollment.Id,
                InternName = intern.FullName,
                TrackName = track.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status.ToString()
            };
        }
    }
}
