using SqlSugar;

namespace SLN.Models;

[SugarTable("Orders")]
public class Order
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public string ReportId { get; set; }
    [SugarColumn(IsOnlyIgnoreInsert = true, IsOnlyIgnoreUpdate = true)]
    public string Lang { get; set; }
}