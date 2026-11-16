using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.UpdateStatusEnrollment.Commands;

public record UpdateStatusEnrollmentCommand(int id) : IRequest<RequestResponse>;
