using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.Tracks.Commands;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.consumers
{
    public class trackcreatedConsumer : IConsumer<trackcreatedmessage>
    {
        private readonly IMediator _mediator;
        public trackcreatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<trackcreatedmessage> context)
        {
            await _mediator.Send(new UpdateTrackCommand(context.Message.Id, context.Message.name, 999 ,true, context.Message.MaxCapacity));
        }
    }
}
//what if consumer fail ? 
//outbox pattern 
