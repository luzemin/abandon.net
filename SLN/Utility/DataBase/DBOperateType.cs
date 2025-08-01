using System.ComponentModel;

namespace SLN.Utility.DataBase;

public enum DBOperateType
{
    [Description("写")] Write,

    [Description("读")] Read
}