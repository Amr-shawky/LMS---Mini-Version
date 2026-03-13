using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orcherstrators;

public record CreateEnrollmentOrcherstratorRequest(int InternId, int TrackId) : IRequest<RequestResult<EnrollmentDto>>;

