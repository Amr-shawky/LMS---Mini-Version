using LMS___Mini_Version.contracts;
using MassTransit;

namespace LMS___Mini_Version.Feature.consumers
{
    public class CashPaymentConsumer : IConsumer<PaymentProcessedMessage>
    {
        private readonly ILogger<PaymentProcessedMessage> _logger;
        public CashPaymentConsumer(ILogger<PaymentProcessedMessage> logger) {

            _logger = logger;
        }
        public Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            Console.WriteLine(
                $"CASH Payment Received: {context.Message.PaymentId}");

            return Task.CompletedTask;
        }
    }
}
