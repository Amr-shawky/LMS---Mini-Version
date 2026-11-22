using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Query
{
    public record CheckInternEmailExistsQuery(string InternEmail) : IRequest<RequestResponse<bool>>;
    
}
