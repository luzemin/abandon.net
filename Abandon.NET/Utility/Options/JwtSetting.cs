using System.ComponentModel.DataAnnotations;

namespace Abandon.NET.Utility.Options;

public class JwtSetting
{
    [Required]
    //[MaxLength(10)]  // .ValidateDataAnnotations().ValidateOnStart(); would verify the attr
    public string Key { get; set; } = string.Empty;
    
    [Required]
    public string Issuer { get; set; } = string.Empty;
    
    [Required]
    public string Audience { get; set; } = string.Empty;
}