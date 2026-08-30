using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record UpdateEnrollmentStatusCommand(int EnrollmentId, EnrollmentStatus NewStatus) : IRequest;
