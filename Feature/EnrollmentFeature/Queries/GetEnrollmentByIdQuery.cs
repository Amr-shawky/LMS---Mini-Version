using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record GetEnrollmentByIdQuery(int EnrollmentId) : IRequest<RequestResponse<EnrollmentDto>>;
}
