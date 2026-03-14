using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentInternCommand;

public record EnrollmentInternCommand(int InternId,int TrackId) : IRequest<EnrollmentResultDto>;