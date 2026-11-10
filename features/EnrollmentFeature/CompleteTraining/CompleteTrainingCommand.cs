using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteTraining
{
    public record CompleteTrainingCommand(int enrollmentId) : IRequest<RequestResponse>;
    
}
