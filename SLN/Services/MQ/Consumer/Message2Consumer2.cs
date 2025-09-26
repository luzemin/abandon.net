using MassTransit;

namespace SLN.Services.MQ;

public class Message2Consumer2 : IConsumer<Message2>
{
    public async Task Consume(ConsumeContext<Message2> context)
    {
        Console.WriteLine("Message2Consumer2 Processing order: " + context.Message.OrderId);
    }
}