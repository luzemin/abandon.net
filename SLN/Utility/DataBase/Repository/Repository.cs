using SqlSugar;

namespace SLN.Utility.DataBase;

public class Repository<T> : IRepository<T> where T : class, new()
{
    private ISqlSugarFactory _factory;

    public Repository(ISqlSugarFactory factory)
    {
        _factory = factory;
    }

    public SimpleClient<T> ReadClient => new(_factory.GetClient());

    public SimpleClient<T> WriteClient => new(_factory.GetClient(DBOperateType.Write));
}