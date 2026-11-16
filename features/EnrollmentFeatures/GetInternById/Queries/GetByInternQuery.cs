using LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.Queries;

public record GetByInternQuery(int internId)
    : IRequest<RequestResponse<IEnumerable<EnrollmentDTO>>>;
