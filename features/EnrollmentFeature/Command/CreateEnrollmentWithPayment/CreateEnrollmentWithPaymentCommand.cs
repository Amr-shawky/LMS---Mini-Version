using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Command.CreateEnrollmentWithPayment;

public record CreateEnrollmentWithPaymentCommand(
    int InternId,
    int TrackId,
    decimal Fees,
    PaymentMethod Method) : IRequest<Enrollment>;