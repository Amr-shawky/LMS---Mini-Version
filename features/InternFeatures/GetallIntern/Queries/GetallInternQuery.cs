using LMS___Mini_Version.features.InternFeatures.GetallIntern.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.GetallIntern.Queries;

public record GetallInternQuery 
    : IRequest<RequestResponse<IEnumerable<InternDTO>>>;