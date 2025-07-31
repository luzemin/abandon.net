using SqlSugar;

namespace SLN.DbHelper;

public interface ISqlSugarFactory
{
    SqlSugarClient GetClient(DBOperateType dBOperate = DBOperateType.Read, DbType dbType = DbType.MySql, string dataBaseName = "Default");
}