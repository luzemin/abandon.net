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

    public OrderService(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetTestAsync()
    {
        return await Task.FromResult("Hello World!");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Order> GetAllAsync()
    {
        var result = await _orderRepository.ReadClient.GetListAsync();
        return result.FirstOrDefault();
    }
}