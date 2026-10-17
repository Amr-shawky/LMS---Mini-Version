using LMS___Mini_Version.contracts;
using MassTransit;

namespace LMS___Mini_Version.Feature.consumers
{
    public class PaymentConfirmedConsumer : IConsumer<PaymentConfirmedMessage>
    {
        public Task Consume(ConsumeContext<PaymentConfirmedMessage> context)
        {
            Console.WriteLine(
            $"CONFIRMED EVENT RECEIVED: {context.Message.PaymentID}");

            return Task.CompletedTask;
        }
    }
}
