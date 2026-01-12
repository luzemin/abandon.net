namespace Abandon.NET.Utility.Redis;

/// <summary>
/// Redis操作接口
/// </summary>
public interface IRedisHelper
{
    /// <summary>
    /// 测试连接
    /// </summary>
    Task<bool> PingAsync();

    /// <summary>
    /// 设置字符串值
    /// </summary>
    Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);

    /// <summary>
    /// 获取字符串值
    /// </summary>
    Task<string?> StringGetAsync(string key);

    /// <summary>
    /// 删除键
    /// </summary>
    Task<bool> KeyDeleteAsync(string key);

    /// <summary>
    /// 检查键是否存在
    /// </summary>
    Task<bool> KeyExistsAsync(string key);

    /// <summary>
    /// 设置过期时间
    /// </summary>
    Task<bool> KeyExpireAsync(string key, TimeSpan? expiry);

    /// <summary>
    /// 获取剩余过期时间
    /// </summary>
    Task<TimeSpan?> KeyTimeToLiveAsync(string key);

    /// <summary>
    /// 获取所有键（支持模式匹配）
    /// </summary>
    Task<List<string>> KeysAsync(string pattern = "*");
}
