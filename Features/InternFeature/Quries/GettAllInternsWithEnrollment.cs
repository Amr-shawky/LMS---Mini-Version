using LMS___Mini_Version.Features.InternFeature.Dtos;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.InternFeature.Quries
{
    public record GettAllInternsWithEnrollment(int pageindex , int pagesize) : IRequest<RequestResponse<IEnumerable<Interndto>>>;
}
