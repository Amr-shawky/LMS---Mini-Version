using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record TransferEnrollmentCommand(int id, int newTrackId) : IRequest;
    
}
