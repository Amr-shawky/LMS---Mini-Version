using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.CQRS.Tracks.Queries.HandlerQueries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.HandlerCommands
{
    public class CreateEnrollmentCommandHandler(IUnitOfWork _unitOfWork, IMediator _mediator) : IRequestHandler<CreateEnrollmentCommand, EnrollInternViewModel>
    {
        public async Task<EnrollInternViewModel> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {
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
            var enrollment = new Enrollment
            {

                InternId = request.internId,
                TrackId = request.trackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active

            };
            _unitOfWork.Enrollments.Add(enrollment);
            await _unitOfWork.CompleteAsync();

            return new EnrollInternViewModel
            {
                InternId = enrollment.InternId,
                TrackId = enrollment.TrackId
            };
        }
    }
}
