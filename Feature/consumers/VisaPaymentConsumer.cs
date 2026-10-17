using LMS___Mini_Version.contracts;
using MassTransit;

namespace LMS___Mini_Version.Feature.consumers
{
    public class VisaPaymentConsumer : IConsumer<PaymentProcessedMessage>
    {
        private readonly ILogger<PaymentProcessedMessage> _logger;
        public VisaPaymentConsumer(ILogger<PaymentProcessedMessage> logger) {

            _logger = logger;
        }
        public Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            int attempt = context.GetRetryAttempt();

            Console.WriteLine($"{DateTime.Now} Attempt Number: {attempt}");

            if (attempt < 2)
            {
                throw new InvalidOperationException(
                    "Temporary payment processing failure");
            }

            Console.WriteLine("VISA Payment Processed Successfully!");

            return Task.CompletedTask;

        }
    }
}
