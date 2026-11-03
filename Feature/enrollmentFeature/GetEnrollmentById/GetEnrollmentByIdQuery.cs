using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.GetEnrollmentById
{
    public record GetEnrollmentByIdQuery(int EnrollmentId) : IRequest<RequestResponse<EnrollmentDto>>;
}
