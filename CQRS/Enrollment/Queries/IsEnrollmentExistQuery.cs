using LMS___Mini_Version.CQRS.RequestResult;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record IsEnrollmentExistQuery(int internId,int trackId) : IRequest<RequestResult<bool>>;
}
