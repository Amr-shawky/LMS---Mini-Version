using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries.GetAllInternQuery;

public record GetAllInternsQuery() : IRequest<IEnumerable<InternDto>>;
