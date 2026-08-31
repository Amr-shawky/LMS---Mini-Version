using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public record TransferEnrollmentOrchestrator(int EnrollmentID, int newTrackID) :IRequest;
    
}
