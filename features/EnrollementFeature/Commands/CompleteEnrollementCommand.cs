using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Commands
{
    public record CompleteEnrollementCommand(int enrollementId) : IRequest<RequestResponse>;
    

    
}
