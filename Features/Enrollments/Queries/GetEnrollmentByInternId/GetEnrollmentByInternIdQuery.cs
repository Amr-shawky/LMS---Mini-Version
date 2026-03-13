using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetEnrollmentByInternId;

public record GetEnrollmentByInternIdQuery(int InternId):IRequest<EnrollmentDto>;