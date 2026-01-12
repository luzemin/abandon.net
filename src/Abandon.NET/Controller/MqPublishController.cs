using Abandon.NET.Services.MQ;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Abandon.NET.Services;

[ApiController]
[Route("api/[controller]")]
public class MqPublishController : ControllerBase
{
    private readonly IBusControl _bus;

    public MqPublishController(IBusControl bus)
    {
        _bus = bus;
    }

    [HttpGet]
    public async Task PublishMessage()
    {
        for (var i = 0; i < 100; i++)
        {
            await _bus.Publish(new Message1() { OrderId = "Message1_" + i });
            await _bus.Publish(new Message2() { OrderId = "Message2_" + i });
        }
    }
}