using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Payments.Queries
{
    //record immutable data type that you cannot make edit on it
    //class 
    public record GetAllPaymentsQuery : IRequest<IEnumerable<PaymentDTO>>;

    public class GetAllPaymentsQueryHandler(IGeneralRepository<Payment> _repository) : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDTO>>
    {
        public async Task<IEnumerable<PaymentDTO>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments =await _repository.GetTable()
                            .Include(P=> P.Enrollment)
                            .ToListAsync(cancellationToken)
                            .ConfigureAwait(false);
            var paymentDtos= payments.Select(p => p.ToPaymentDto());
            return paymentDtos;

           
        }
    }

}
