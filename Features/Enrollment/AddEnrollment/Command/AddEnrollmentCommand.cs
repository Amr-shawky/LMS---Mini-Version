using LMS___Mini_Version.Common;
using MediatR;
using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Features.Enrollment.AddEnrollment.Command
{
    public record AddEnrollmentCommand(int internId, int trackId) : IRequest<int>;
    public class AddEnrollmentCommandHandler : IRequestHandler<AddEnrollmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddEnrollmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = new LMS___Mini_Version.Domain.Entities.Enrollment()
            {
                TrackId = request.trackId,
                InternId = request.internId,
                

            };
            await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().AddAsync(enrollment);
            await _unitOfWork.SaveChagesAsync();
            return enrollment.Id;
        }

        
    }
}
