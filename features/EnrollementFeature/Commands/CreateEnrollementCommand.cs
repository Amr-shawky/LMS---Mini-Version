using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Commands
{
    public record CreateEnrollementCommand (int internId , int trackId ,  EnrollmentStatus status , DateTime date) :IRequest<RequestResponse<CreateEnrollementDTO>>;
    
    
}
