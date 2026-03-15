using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollment.Queries.Handlers
{
    public class GetByInternIdQueryHandler : IRequestHandler<GetByInternIdQuery, IEnumerable<EnrollmentViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetByInternIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<EnrollmentViewModel>> Handle(GetByInternIdQuery request, CancellationToken cancellationToken)


        {
            var enrollments = await _unitOfWork.Enrollments
                .GetTable()
                .Where(e => e.InternId == request.InternId)
                .Select(e => new EnrollmentViewModel
                {
                    Id = e.Id,
                    InternName = e.Intern.FullName,
                    TrackName = e.Track.Name,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status.ToString()
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if(!enrollments.Any())
                throw new NotFoundException("Enrollments", request.InternId);
            return enrollments;
        }
    }

}
