using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollments;

public record GetAllEnrollmentsQuery() : IRequest<IEnumerable<EnrollmentDto>>;