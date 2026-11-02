using LMS___Mini_Version.Features.Enrollment.Contract;
using LMS___Mini_Version.Features.Payment.RefunPayment.Command;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.TransferEnrollment.Consumer
{
    public class EnrollmentTransferredConsumer : IConsumer<EnrollmentTransferedMessage>
    {
        private readonly IMediator _mediator;
        public EnrollmentTransferredConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<EnrollmentTransferedMessage> context)
        {
            await _mediator.Send(new RefundPaymentCommand(context.Message.EnrollmentId));

            
        }
    }
}
