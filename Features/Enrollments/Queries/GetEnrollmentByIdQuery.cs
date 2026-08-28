
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries;

public record GetEnrollmentByIdQuery(int Id) : IRequest<EnrollmentDto?>;