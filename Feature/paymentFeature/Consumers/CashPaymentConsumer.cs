using LMS___Mini_Version.Feature.paymentFeature.Events;
using MassTransit;

namespace LMS___Mini_Version.Feature.paymentFeature.Consumers
{
    public class CashPaymentConsumer : IConsumer<PaymentProcessedMessage>
    {
        private readonly ILogger<CashPaymentConsumer> _logger;

        public CashPaymentConsumer(ILogger<CashPaymentConsumer> logger)
            => _logger = logger;

        public async Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            int attempt = context.GetRetryAttempt();

            _logger.LogInformation("Attempt: {Attempt} at {Time} for Message: {@Message}",
                attempt,
                DateTime.Now,
                context.Message);
            await context.Publish(new PaymentConfirmedMessage(context.Message.PaymentId));

            if (attempt < 2)
            {
                _logger.LogWarning("Simulating a failure to trigger Retry...");
                throw new InvalidOperationException("error");
            }

            _logger.LogInformation("Message processed successfully on attempt {Attempt}!", attempt);

            await Task.CompletedTask;
        }
    }
}