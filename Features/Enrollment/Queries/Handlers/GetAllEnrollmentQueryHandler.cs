using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Queries.Handlers
{
    public class GetAllEnrollmentQueryHandler : IRequestHandler<GetAllEnrollmentQuery, IEnumerable<EnrollmentViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEnrollmentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<EnrollmentViewModel>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();

           
            var enrollmentViewModels = enrollments.Select(e => new EnrollmentViewModel
            {
                Id = e.Id,
                InternName = e.Intern?.FullName ?? "N/A",  
                EnrollmentDate = e.EnrollmentDate,
                Status = e?.Status != null ? e.Status.ToString() : "Unknown" 
            }).ToList();

            return enrollmentViewModels;
        }
    }
}
