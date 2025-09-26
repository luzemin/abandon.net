using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace SLN.Services.MQ;

[DynamicWebApi]
[AllowAnonymous]
public class Publisher : IDynamicWebApi
{
    private readonly IBusControl _bus;

    public Publisher(IBusControl bus)
    {
        _bus = bus;
    }

    [HttpGet]
    public async Task PublishMessage(string orderId)
    {
        await _bus.Publish(new Message1() { OrderId = orderId });
        await _bus.Publish(new Message2() { OrderId = orderId });
        Console.WriteLine(" [x] Sent order: {0}", orderId);
    }
}