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
        public async Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            Console.WriteLine("1. Cash Consumer Started");

            await context.Publish(
                new PaymentConfirmedMessage(context.Message.PaymentId));

            Console.WriteLine("2. Event Published");

            // Simulate a failed database SaveChanges
            throw new InvalidOperationException(
                "Database SaveChanges Failed!");
        }
    }
}
