using SqlSugar;

namespace SLN.DbHelper;

public class Repository<T> : IRepository<T> where T : class, new()
{
    private ISqlSugarFactory _factory;

    public Repository(ISqlSugarFactory factory)
    {
        _factory = factory;
    }

    public SimpleClient<T> ReadClient => new SimpleClient<T>(_factory.GetClient());

    public SimpleClient<T> WriteClient => new SimpleClient<T>(_factory.GetClient(DBOperateType.Write));
}