using LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.Query
{
    public record InternRegistrationApprovalQuery(int InternId, InternRegistrationApprovalDTO InternRegistrationApprovalDTO) : IRequest<RequestResponse>;
}