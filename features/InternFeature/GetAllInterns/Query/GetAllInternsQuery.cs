using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Query
{
    public record GetAllInternsQuery
        : IRequest<RequestResponse<IEnumerable<InternDto>>>;
}
