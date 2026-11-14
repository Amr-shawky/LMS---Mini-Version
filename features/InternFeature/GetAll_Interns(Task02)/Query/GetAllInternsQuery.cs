using LMS___Mini_Version.features.InternFeature.GetAll_Interns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetAll_Interns.Query
{

    public class GetAllInternsQuery: IRequest<RequestResponse<IEnumerable<InternDto>>>
    {
    }
}
