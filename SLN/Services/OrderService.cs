using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using SLN.Models;
using SLN.Utility.DataBase;
using SLN.Utility.Options;

namespace SLN.Services;

[DynamicWebApi]
public class OrderService : IDynamicWebApi
{
    private readonly IRepository<Order> _orderRepository;
    private readonly ISqlSugarFactory _sqlSugarFactory;
    private readonly JwtSetting _jwtSetting;

    public OrderService(IRepository<Order> orderRepository, ISqlSugarFactory sqlSugarFactory, IOptions<JwtSetting> jwtSetting)
    {
        _orderRepository = orderRepository;
        _sqlSugarFactory = sqlSugarFactory;
        _jwtSetting = jwtSetting.Value;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetTestAsync()
    {
        return await Task.FromResult("Hello World!");
    }

    [HttpGet]
    [AllowAnonymous]
    public Task<string> GetTestExceptionAsync()
    {
        throw new Exception("test exception");
    }

    [HttpGet]
    //[Authorize]
    //[Authorize(Roles = "Admin")]
    [Authorize(Policy = "MustBeEmployee")]
    public async Task<string> GetTestAuthAsync()
    {
        return await Task.FromResult("Hello World!");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Order> GetAllAsync()
    {
        //通用client 示例
        await _sqlSugarFactory.GetClient().Ado.ExecuteCommandAsync("select 1");

        //单表client 示例
        var result = await _orderRepository.ReadClient.GetListAsync();
        return result.FirstOrDefault();
    }
    
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<JwtSetting> GetJwtOptionAsync()
    {
        return await Task.FromResult(_jwtSetting);
    }
}