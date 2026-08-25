using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record GetEnrollmentByIdQuery(int EnrollmentId) : IRequest<EnrollmentDto>;

}
