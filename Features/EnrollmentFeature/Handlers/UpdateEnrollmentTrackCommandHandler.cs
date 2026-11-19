using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.EnrollmentFeature.Commands;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Handlers
{
    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<bool>> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _repository.GetByIdAsync(request.EnrollmentID);

            enrollment.TrackId = request.TrackID;

            _repository.Update(enrollment);



            return RequestResponse<bool>.OK(202,true);

        }
    }
}
