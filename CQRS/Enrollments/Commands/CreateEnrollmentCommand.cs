using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands;

public record CreateEnrollmentCommand(int InternId, int TrackId) : IRequest<EnrollmentDto>;



