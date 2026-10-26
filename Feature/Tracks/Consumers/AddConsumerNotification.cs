using LMS___Mini_Version.Feature.Tracks.Events;
using MassTransit;

namespace LMS___Mini_Version.Feature.Tracks.Consumers
{
    public class AddConsumerNotification : IConsumer<TrackCreatedMessage>
    {
        private readonly ILogger<TrackCreatedMessage> _logger;

        public AddConsumerNotification(ILogger<TrackCreatedMessage> logger)
        
        =>  _logger = logger;
        
        public Task Consume(ConsumeContext<TrackCreatedMessage> context)
        {
            Console.WriteLine($"New Track Created with ID {context.Message.TrackId}");
            //_logger.LogWarning($"New Track Created with ID {context.Message.TrackId}");
            return Task.CompletedTask;
        }
    }
}
