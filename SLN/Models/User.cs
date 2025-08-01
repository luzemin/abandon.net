using SqlSugar;

namespace SLN.Models;

[SugarTable("User")]
public class User
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}