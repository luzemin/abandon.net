using System.ComponentModel;

namespace Abandon.NET.Utility.DataBase;

public enum DBOperateType
{
    [Description("写")] Write,

    [Description("读")] Read
}