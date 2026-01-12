using StackExchange.Redis;

namespace Abandon.NET.Utility.Redis;

/// <summary>
/// Redis操作帮助类
/// </summary>
public class RedisHelper : IRedisHelper
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    public RedisHelper(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }

    /// <summary>
    /// 测试连接
    /// </summary>
    public async Task<bool> PingAsync()
    {
        try
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            if (endpoints.Length == 0) return false;

            var server = _connectionMultiplexer.GetServer(endpoints[0]);
            var result = await server.PingAsync();
            return result != TimeSpan.MaxValue;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 设置字符串值
    /// </summary>
    public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _database.StringSetAsync(key, value, expiry);
    }

    /// <summary>
    /// 获取字符串值
    /// </summary>
    public async Task<string?> StringGetAsync(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    /// <summary>
    /// 删除键
    /// </summary>
    public async Task<bool> KeyDeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 检查键是否存在
    /// </summary>
    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    /// <summary>
    /// 设置过期时间
    /// </summary>
    public async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry)
    {
        return await _database.KeyExpireAsync(key, expiry);
    }

    /// <summary>
    /// 获取剩余过期时间
    /// </summary>
    public async Task<TimeSpan?> KeyTimeToLiveAsync(string key)
    {
        return await _database.KeyTimeToLiveAsync(key);
    }

    /// <summary>
    /// 获取所有键（支持模式匹配）
    /// </summary>
    public async Task<List<string>> KeysAsync(string pattern = "*")
    {
        var keys = new List<string>();
        var endpoints = _connectionMultiplexer.GetEndPoints();
        
        foreach (var endpoint in endpoints)
        {
            var server = _connectionMultiplexer.GetServer(endpoint);
            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                keys.Add(key.ToString());
            }
        }
        
        return keys;
    }
}
