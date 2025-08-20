using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace SLN.Services;

[McpServerToolType]
[DynamicWebApi]
public class McpToolService : IDynamicWebApi
{
    [HttpGet]
    [AllowAnonymous]
    [McpServerTool, Description("echo message back to the client")]
    public Task<string> Echo(string input)
    {
        return Task.FromResult(input);
    }
}