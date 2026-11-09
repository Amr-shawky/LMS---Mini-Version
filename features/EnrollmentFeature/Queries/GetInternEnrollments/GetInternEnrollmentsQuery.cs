using LMS___Mini_Version.features.EnrollmentFeature.DTOs;

using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.GetInternEnrollments;

public record GetInternEnrollmentsQuery(int InternId) : IRequest<RequestResponse<List<EnrollmentDto>>>;
