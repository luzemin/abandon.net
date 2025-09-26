using MassTransit;

namespace Abandon.NET.Services.MQ;

public class Message1Consumer1 : IConsumer<Message1>
{
    private readonly IServiceProvider _serviceProvider;
    
    public Message1Consumer1(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task Consume(ConsumeContext<Message1> context)
    {
        Console.WriteLine("Message1Consumer1 Processing order: " + context.Message.OrderId);
    }
}