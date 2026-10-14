using LMS___Mini_Version.Feature.paymentFeature.Enents;
using MassTransit;

namespace LMS___Mini_Version.Feature.paymentFeature.Consumers
{
    public class CashPaymentConsumer : IConsumer<PaymentProcessedMessage>
    {
        private readonly ILogger<PaymentProcessedMessage> _logger;

        public CashPaymentConsumer(ILogger<PaymentProcessedMessage> logger)
            =>_logger = logger;
        public async Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            _logger.LogInformation("CashPaymentConsumer received message: {Message}", context.Message);
            await Task.CompletedTask;
        }
    }
}
