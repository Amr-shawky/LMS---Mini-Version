using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetEnrollmentForCompletationQuey(int enrollmentId) : IRequest<RequestResponse<Enrollment>>;

    public class GetEnrollmentForCompletationQueyHandler : IRequestHandler<GetEnrollmentForCompletationQuey,RequestResponse<Enrollment>>
    {
        private IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetEnrollmentForCompletationQueyHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        
        public async Task<RequestResponse<Enrollment>> Handle(GetEnrollmentForCompletationQuey query,CancellationToken cancellationToken)
        {
            var enrollments = _enrollmentRepo.GetById(query.enrollmentId);
            return RequestResponse<Enrollment>.OK(enrollments);
        }
    }


}
