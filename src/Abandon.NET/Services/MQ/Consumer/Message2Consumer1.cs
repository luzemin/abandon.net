using MassTransit;

namespace Abandon.NET.Services.MQ;

public class Message2Consumer1 : IConsumer<Message2>
{
    public async Task Consume(ConsumeContext<Message2> context)
    {
        Console.WriteLine("Message2Consumer1 Processing order: " + context.Message.OrderId);
    }
}