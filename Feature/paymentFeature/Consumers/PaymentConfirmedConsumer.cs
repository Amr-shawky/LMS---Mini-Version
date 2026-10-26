using LMS___Mini_Version.Feature.paymentFeature.Events;
using MassTransit;

namespace LMS___Mini_Version.Feature.paymentFeature.Consumers
{
    public class PaymentConfirmedConsumer : IConsumer<PaymentConfirmedMessage>
    {
        public Task Consume(ConsumeContext<PaymentConfirmedMessage> context)
        {
            Console.WriteLine($"PaymentConfirmed Succcess With Id {context.Message.PaymentId}");
            return Task.CompletedTask;
        }
    }
}
