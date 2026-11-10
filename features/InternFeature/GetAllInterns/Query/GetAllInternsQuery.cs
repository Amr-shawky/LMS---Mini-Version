using LMS___Mini_Version.features.InternFeature.GetAllInterns.DTOS;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Query
{
    public record GetAllInternsQuery :
        IRequest<RequestResponse<List<GetAllInternsDTO>>>;
}
