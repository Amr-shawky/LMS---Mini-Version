using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries.GetInternByIdQuery;

public record GetInternByIdQuery(int Id) : IRequest<InternDto?>;