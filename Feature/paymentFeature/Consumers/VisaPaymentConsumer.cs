using LMS___Mini_Version.Feature.paymentFeature.Events;
using MassTransit;

namespace LMS___Mini_Version.Feature.paymentFeature.Consumers
{
    public class VisaPaymentConsumer : IConsumer<PaymentProcessedMessage>
    {
        private readonly ILogger<PaymentProcessedMessage> _logger;

        public VisaPaymentConsumer(ILogger<PaymentProcessedMessage> logger)
            =>_logger = logger;
        public async Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            _logger.LogInformation("VisaPaymentConsumer received message: {Message}", context.Message);
            await Task.CompletedTask;
        }
    }   
}
