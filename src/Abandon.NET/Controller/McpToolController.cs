using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;

namespace Abandon.NET.Services;

[McpServerToolType]
[ApiController]
[Route("api/[controller]")]
public class McpToolController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [McpServerTool, Description("echo message back to the client")]
    public Task<string> Echo(string input)
    {
        return Task.FromResult(input);
    }
}