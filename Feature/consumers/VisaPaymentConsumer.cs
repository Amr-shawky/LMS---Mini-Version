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
            var attempts = context.GetRetryAttempt();

            Console.WriteLine($"payment id :{context.Message.PaymentId}" +
                $"retry number {attempts}");
            //Console.WriteLine(
            //    $"VISA Payment Received: {context.Message.PaymentId}");

            //_logger.LogWarning($"VISA Payment Received: {context.Message.PaymentId}");

            if (attempts < 2)
            {
            throw new InvalidOperationException(
            "Testing MassTransit failure handling");

            }
            Console.WriteLine("Payment Processed Successfully!");


            return Task.CompletedTask;

        }
    }
}
