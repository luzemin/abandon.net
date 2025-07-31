using System.ComponentModel;

namespace SLN.DbHelper;

public enum DBOperateType
{
    [Description("写")] Write,

    [Description("读")] Read
}