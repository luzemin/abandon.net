using SqlSugar;

namespace SLN.DbHelper;

public interface IRepository<T> where T : class, new()
{
    public SimpleClient<T> ReadClient { get; }

    public SimpleClient<T> WriteClient { get; }
}