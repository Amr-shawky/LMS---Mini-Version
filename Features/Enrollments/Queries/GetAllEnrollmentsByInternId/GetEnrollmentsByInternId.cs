using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollmentsByInternId;

public record GetEnrollmentsByInternId(int InternId):IRequest<IEnumerable<EnrollmentDto>>;