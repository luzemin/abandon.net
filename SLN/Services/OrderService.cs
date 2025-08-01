using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using SLN.DbHelper;
using SLN.Models;

namespace SLN.Services;

[DynamicWebApi]
public class OrderService : IDynamicWebApi
{
    private IRepository<Order> _orderRepository;
    private ISqlSugarFactory _sqlSugarFactory;

    public OrderService(IRepository<Order> orderRepository, ISqlSugarFactory sqlSugarFactory)
    {
        _orderRepository = orderRepository;
        _sqlSugarFactory = sqlSugarFactory;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetTestAsync()
    {
        return await Task.FromResult("Hello World!");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetTestExceptionAsync()
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
}