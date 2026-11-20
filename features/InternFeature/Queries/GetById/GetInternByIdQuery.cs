using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries.GetById;

public record GetInternByIdQuery(int InternId) : IRequest<Intern?>;