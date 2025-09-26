using MassTransit;

namespace Abandon.NET.Services.MQ;

public class Message1Consumer2 : IConsumer<Message1>
{
    public async Task Consume(ConsumeContext<Message1> context)
    {
        Console.WriteLine("Message1Consumer2 Processing order: " + context.Message.OrderId);
    }
}