using LMS___Mini_Version.contracts;
using MassTransit;

namespace LMS___Mini_Version.Feature.consumers
{
    public class NotificationConsumer : IConsumer<trackcreatedmessage>
    {
        private readonly ILogger<NotificationConsumer> _logger;
        public NotificationConsumer(ILogger<NotificationConsumer> logger)
        {
            _logger = logger; 
        }
        public async Task Consume(ConsumeContext<trackcreatedmessage> context ) 
        {
            _logger.LogInformation($"track name : {context.Message.name}");
        }
    }
}
