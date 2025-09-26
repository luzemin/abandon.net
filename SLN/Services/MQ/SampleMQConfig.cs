using MassTransit;

namespace SLN.Services.MQ;

public static class SampleMQConfig
{
    public static void AddMassTransitRabbitMQ(this IServiceCollection service, IConfiguration configuration)
    {
        var mqInfo = configuration.GetSection("MQConfig").Get<MQConfig>();
        service.AddMassTransit(x =>
        {
            // Add Consumers
            x.AddConsumer<Message1Consumer1>(configurator =>
            {
                //重试策略
                // 5次重试，每次间隔1秒，最大间隔30秒，每次间隔增加2秒
                configurator.UseMessageRetry(r => r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));
            });
            x.AddConsumer<Message1Consumer2>();
            x.AddConsumer<Message2Consumer1>();
            x.AddConsumer<Message2Consumer2>();

            x.UsingRabbitMq((context, cfg) =>
            {
                // Host
                cfg.Host(mqInfo.HostName, (ushort)mqInfo.Port, mqInfo.VirtualHost, mqInfo.HostName, c =>
                {
                    c.Username(mqInfo.UserName);
                    c.Password(mqInfo.Password);
                });

                // Consumer
                cfg.ReceiveEndpoint("m1c1_queue", e => { e.ConfigureConsumer<Message1Consumer1>(context); });
                cfg.ReceiveEndpoint("m1c2_queue", e => { e.ConfigureConsumer<Message1Consumer2>(context); });
                cfg.ReceiveEndpoint("m2c1_queue", e => { e.ConfigureConsumer<Message2Consumer1>(context); });
                cfg.ReceiveEndpoint("m2c2_queue", e => { e.ConfigureConsumer<Message2Consumer2>(context); });
            });
        });
    }
}