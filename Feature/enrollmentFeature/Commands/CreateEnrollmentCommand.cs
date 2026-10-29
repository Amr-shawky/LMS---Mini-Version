using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record CreateEnrollmentCommand(int InternId, int TrackId) : IRequest<RequestResponse<int>>;
}
