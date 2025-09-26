namespace Abandon.NET.Services.MQ;

public class MQConfig
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string VirtualHost { get; set; }
    public ushort PrefetchCount { get; set; }
}