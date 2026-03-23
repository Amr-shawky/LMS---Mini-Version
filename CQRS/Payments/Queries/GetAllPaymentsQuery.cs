using LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Queries
{
    //record immutable data type that you cannot make edit on it
    //class 
    public record GetAllPaymentsQuery : IRequest<IEnumerable<PaymentDTO>>;

    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDTO>>
    {
        public Task<IEnumerable<PaymentDTO>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            // Implement the logic to retrieve all payments and return them as a list of PaymentDTO
            throw new NotImplementedException();
        }
    }

}
