using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Queries
{
    public record CheckInternEmailExistsQuery(string Email) : IRequest<RequestResponse<bool>>;
}
