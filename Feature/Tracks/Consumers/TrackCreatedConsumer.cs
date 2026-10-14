using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Events;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Consumers
{
    public class TrackCreatedConsumer : IConsumer<TrackCreatedMessage>
    {
        private readonly IMediator _mediator;
        public TrackCreatedConsumer(IMediator mediator)
           => _mediator = mediator;
        public async Task Consume(ConsumeContext<TrackCreatedMessage> context)
           => await _mediator.Send(new UpdateTrackCommand(
                 context.Message.TrackId,
                 context.Message.Name,
                 1000,
                 true,
                 400));

    }
}
