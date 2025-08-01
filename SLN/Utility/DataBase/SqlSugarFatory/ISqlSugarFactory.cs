using SqlSugar;

namespace SLN.Utility.DataBase;

public interface ISqlSugarFactory
{
    SqlSugarClient GetClient(DBOperateType dBOperate = DBOperateType.Read, DbType dbType = DbType.MySql, string dataBaseName = "Default");
}