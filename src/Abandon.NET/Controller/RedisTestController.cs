using Abandon.NET.Utility.Redis;
using Microsoft.AspNetCore.Mvc;

namespace Abandon.NET.Controller;

/// <summary>
/// Redis测试控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RedisTestController : ControllerBase
{
    private readonly IRedisHelper _redisHelper;

    public RedisTestController(IRedisHelper redisHelper)
    {
        _redisHelper = redisHelper;
    }

    /// <summary>
    /// 测试Redis连通性
    /// </summary>
    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        var isConnected = await _redisHelper.PingAsync();
        return Ok(new
        {
            IsConnected = isConnected,
            Message = isConnected ? "Redis连接成功" : "Redis连接失败",
            Timestamp = DateTime.Now
        });
    }

    /// <summary>
    /// 设置键值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expirySeconds">过期时间（秒）</param>
    [HttpPost("set")]
    public async Task<IActionResult> Set([FromQuery] string key, [FromQuery] string value, [FromQuery] int? expirySeconds = null)
    {
        TimeSpan? expiry = expirySeconds.HasValue ? TimeSpan.FromSeconds(expirySeconds.Value) : null;
        var result = await _redisHelper.StringSetAsync(key, value, expiry);
        
        return Ok(new
        {
            Success = result,
            Key = key,
            Value = value,
            ExpirySeconds = expirySeconds,
            Message = result ? "设置成功" : "设置失败"
        });
    }

    /// <summary>
    /// 获取键值
    /// </summary>
    /// <param name="key">键</param>
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] string key)
    {
        var value = await _redisHelper.StringGetAsync(key);
        var exists = await _redisHelper.KeyExistsAsync(key);
        
        return Ok(new
        {
            Key = key,
            Value = value,
            Exists = exists,
            Message = exists ? "键存在" : "键不存在"
        });
    }

    /// <summary>
    /// 删除键
    /// </summary>
    /// <param name="key">键</param>
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] string key)
    {
        var result = await _redisHelper.KeyDeleteAsync(key);
        
        return Ok(new
        {
            Success = result,
            Key = key,
            Message = result ? "删除成功" : "删除失败（键可能不存在）"
        });
    }

    /// <summary>
    /// 检查键是否存在
    /// </summary>
    /// <param name="key">键</param>
    [HttpGet("exists")]
    public async Task<IActionResult> Exists([FromQuery] string key)
    {
        var exists = await _redisHelper.KeyExistsAsync(key);
        
        return Ok(new
        {
            Key = key,
            Exists = exists,
            Message = exists ? "键存在" : "键不存在"
        });
    }

    /// <summary>
    /// 设置键的过期时间
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="expirySeconds">过期时间（秒）</param>
    [HttpPost("expire")]
    public async Task<IActionResult> Expire([FromQuery] string key, [FromQuery] int expirySeconds)
    {
        var result = await _redisHelper.KeyExpireAsync(key, TimeSpan.FromSeconds(expirySeconds));
        
        return Ok(new
        {
            Success = result,
            Key = key,
            ExpirySeconds = expirySeconds,
            Message = result ? "设置过期时间成功" : "设置过期时间失败（键可能不存在）"
        });
    }

    /// <summary>
    /// 获取键的剩余过期时间
    /// </summary>
    /// <param name="key">键</param>
    [HttpGet("ttl")]
    public async Task<IActionResult> GetTimeToLive([FromQuery] string key)
    {
        var ttl = await _redisHelper.KeyTimeToLiveAsync(key);
        
        return Ok(new
        {
            Key = key,
            TimeToLive = ttl,
            TotalSeconds = ttl?.TotalSeconds,
            Message = ttl.HasValue 
                ? (ttl.Value == TimeSpan.MaxValue ? "键永不过期" : $"剩余 {ttl.Value.TotalSeconds:F2} 秒")
                : "键不存在"
        });
    }

    /// <summary>
    /// 获取所有键
    /// </summary>
    /// <param name="pattern">匹配模式（默认为*，表示所有键）</param>
    [HttpGet("keys")]
    public async Task<IActionResult> GetKeys([FromQuery] string pattern = "*")
    {
        var keys = await _redisHelper.KeysAsync(pattern);
        
        return Ok(new
        {
            Pattern = pattern,
            Count = keys.Count,
            Keys = keys
        });
    }

    /// <summary>
    /// 批量操作测试（创建、读取、更新、删除）
    /// </summary>
    [HttpPost("crud-test")]
    public async Task<IActionResult> CrudTest()
    {
        var testKey = $"test:crud:{Guid.NewGuid()}";
        var testValue1 = "初始值";
        var testValue2 = "更新后的值";

        // Create
        var createResult = await _redisHelper.StringSetAsync(testKey, testValue1, TimeSpan.FromMinutes(5));
        
        // Read
        var readValue1 = await _redisHelper.StringGetAsync(testKey);
        
        // Update
        var updateResult = await _redisHelper.StringSetAsync(testKey, testValue2, TimeSpan.FromMinutes(5));
        var readValue2 = await _redisHelper.StringGetAsync(testKey);
        
        // Check TTL
        var ttl = await _redisHelper.KeyTimeToLiveAsync(testKey);
        
        // Delete
        var deleteResult = await _redisHelper.KeyDeleteAsync(testKey);
        var existsAfterDelete = await _redisHelper.KeyExistsAsync(testKey);

        return Ok(new
        {
            TestKey = testKey,
            CreateSuccess = createResult,
            ReadValue1 = readValue1,
            UpdateSuccess = updateResult,
            ReadValue2 = readValue2,
            TimeToLiveSeconds = ttl?.TotalSeconds,
            DeleteSuccess = deleteResult,
            ExistsAfterDelete = existsAfterDelete,
            Message = "CRUD测试完成"
        });
    }
}
