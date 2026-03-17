using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands.CancelEnrollment;

public record CancelEnrollmentCommand(int EnrollmentId) : IRequest<EnrollmentResultDto>;