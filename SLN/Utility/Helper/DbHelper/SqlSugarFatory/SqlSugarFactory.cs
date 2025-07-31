using SqlSugar;
using SLN.LogHelper;
using static SLN.DbHelper.ConnectionStringProvider;

namespace SLN.DbHelper;

public class SqlSugarFactory : ISqlSugarFactory
{
    private INLogHelper _NLogHelper;
    private List<ConnectionStringItem> _connectionConfigs;

    public SqlSugarFactory(INLogHelper nLogHelper, IConfiguration configuration)
    {
        _NLogHelper = nLogHelper;
        _connectionConfigs = configuration.GetSection("ConnectionStrings").Get<List<ConnectionStringItem>>();
    }

    public SqlSugarClient GetClient(DBOperateType dBOperate = DBOperateType.Read, DbType dbType = DbType.MySql, string dataBaseName = "Default")
    {
        //创建数据库对象
        var db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = GetConnectionString(_connectionConfigs, dataBaseName, dBOperate == DBOperateType.Write), //连接符字串
            DbType = dbType,
            IsAutoCloseConnection = true,
            ConfigId = dataBaseName,
            InitKeyType = InitKeyType.Attribute //从特性读取主键自增信息
        });

        //添加Sql打印事件，监控所有超过5秒的Sql
        db.Aop.OnLogExecuted = (sql, p) =>
        {
            //执行时间超过5秒
            if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
            {
                //代码CS文件名
                var fileName = db.Ado.SqlStackTrace.FirstFileName;
                //代码行数
                var fileLine = db.Ado.SqlStackTrace.FirstLine;
                //方法名
                var FirstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                //db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息

                _NLogHelper.Info($"监控所有超过5秒的Sql+[ {sql} ],参数：[ {string.Join(";", p.Select(e => e.Value))}]位置：{fileName},{fileLine},{FirstMethodName}");
            }
        };
        return db;
    }
}