using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries.GetAll;

public record GetAllInternsQuery : IRequest<RequestResponse<List<InternDto>>>;
