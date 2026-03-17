using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;

public record EnrollmentInternCommand(int InternId,int TrackId) : IRequest<EnrollmentResultDto>;