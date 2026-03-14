using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries.GetEnrollmentsByTrackId;

public record GetEnrollmentsByTrackIdQuery(int TrackId):IRequest<IEnumerable<EnrollmentDto>>;