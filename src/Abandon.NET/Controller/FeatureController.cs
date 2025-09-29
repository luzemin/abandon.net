using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Abandon.NET.Services;

/// <summary>
/// https://github.com/microsoft/FeatureManagement-Dotnet
/// https://www.milanjovanovic.tech/blog/feature-flags-in-dotnet-and-how-i-use-them-for-ab-testing?utm_source=X&utm_medium=social&utm_campaign=29.09.2025
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FeatureController : ControllerBase
{
    private readonly IFeatureManager _featureManager;

    public FeatureController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetContent()
    {
        var content = "Feature management provides a way to develop and expose application functionality based on features. Many applications have special requirements when a new feature is developed such as when the feature should be enabled and under what conditions. This library provides a way to define these relationships, and also integrates into common .NET code patterns to make exposing these features possible.";

        if (await _featureManager.IsEnabledAsync("ClipArticleContent"))
        {
            return content.Substring(0, 50) + "...";
        }

        return content;
    }
}