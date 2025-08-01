using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using SLN.Dto;

namespace SLN.Services;

[DynamicWebApi]
public class UserService : IDynamicWebApi
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [AllowAnonymous]
    public dynamic Login([FromBody] LoginRequest request)
    {
        request.Username = "Timothy";

        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, "123"),
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, "Employee"),
            new("CustomClaimType", "CustomValue"), // 示例自定义声明
            new("EmployeeId", "E001")
        ];

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // 令牌有效期30分钟
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        ));

        return new { Token = tokenString };
    }
}