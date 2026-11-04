using LMS___Mini_Version.Messaging;
using MassTransit;

namespace LMS___Mini_Version.Feature
{
    public class createtrackconsumer : IConsumer<EnrollmentTransferred>
    {
        public Task Consume(ConsumeContext<EnrollmentTransferred> context)
        {
            
        }
    }
}
