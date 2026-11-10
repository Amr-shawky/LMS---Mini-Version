using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Commands
{
    public record ApproveInternCommand(int internId) : IRequest<RequestResponse>;
    
  
}
