using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Command
{
    public record CreateNewEnrollmentCommand(int trackId,int internId) : IRequest<Enrollment>;
   
}
