using LMS___Mini_Version.Features.EnrollmentFeature.Dtos;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Quries
{
    public record GetEnrollmentByIdQuery(int ID) : IRequest<RequestResponse<EnrollmentDto>>;

}
