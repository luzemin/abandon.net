namespace SLN.DbHelper;

public class ConnectionStringItem
{
    /// <summary>
    /// 连接字符串名字-数据库名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 写连接字符串
    /// </summary>
    public string Write { get; set; }

    /// <summary>
    /// 读连接字符串列表
    /// </summary>
    public string[] Read { get; set; }

    /// <summary>
    /// 读连接字符串权重， 0表示暂时忽略
    /// </summary>
    public int[] ReadWeight { get; set; }
}