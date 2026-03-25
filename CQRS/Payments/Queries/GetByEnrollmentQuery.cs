using LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Queries
{
    public record GetByEnrollmentQuery(int enrollmentId) : IRequest<PaymentDTO>;
   
}
