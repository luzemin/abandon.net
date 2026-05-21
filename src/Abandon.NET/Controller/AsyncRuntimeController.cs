using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abandon.NET.Services;

/// <summary>
/// Demonstrate .NET 11 runtime-async feature.
/// When &lt;Features&gt;runtime-async=on&lt;/Features&gt; is set in csproj,
/// async stack traces are significantly shorter and cleaner.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AsyncRuntimeController : ControllerBase
{
    /// <summary>
    /// Trigger a deep async call chain and return the captured stack trace.
    /// Compare the result with runtime-async=on vs off:
    ///   - ON:  clean stack, only your method names, very few frames
    ///   - OFF: noisy stack with MoveNext, AsyncTaskMethodBuilder, etc.
    /// </summary>
    [HttpGet("stack-trace")]
    [AllowAnonymous]
    public async Task<object> GetAsyncStackTrace()
    {
        try
        {
            await Level1Async();
            return new { error = "Expected exception was not thrown" };
        }
        catch (Exception ex)
        {
            var lines = ex.StackTrace?
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? [];

            return new
            {
                feature = "runtime-async",
                // Check csproj: <Features>runtime-async=on</Features>
                description = "When runtime-async is ON, the stack trace below should be short and clean. " +
                              "When OFF, you will see extra frames like MoveNext, AsyncTaskMethodBuilder, etc.",
                frameCount = lines.Length,
                stackTrace = lines
            };
        }
    }

    /// <summary>
    /// Compare two async patterns side-by-side:
    ///   1. Normal async/await chain
    ///   2. ContinueWith chain (not affected by runtime-async)
    /// </summary>
    [HttpGet("compare")]
    [AllowAnonymous]
    public async Task<object> CompareAsyncPatterns()
    {
        // Pattern 1: async/await — benefits from runtime-async
        string[] awaitStack;
        try
        {
            await Level1Async();
            awaitStack = [];
        }
        catch (Exception ex)
        {
            awaitStack = ex.StackTrace?
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? [];
        }

        // Pattern 2: ContinueWith — always has extra frames
        string[] continueWithStack;
        try
        {
            await ContinueWithChainAsync();
            continueWithStack = [];
        }
        catch (Exception ex)
        {
            continueWithStack = ex.StackTrace?
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? [];
        }

        return new
        {
            feature = "runtime-async comparison",
            asyncAwait = new
            {
                description = "async/await chain — runtime-async makes this stack shorter",
                frameCount = awaitStack.Length,
                stackTrace = awaitStack
            },
            continueWith = new
            {
                description = "ContinueWith chain — not optimized by runtime-async, always verbose",
                frameCount = continueWithStack.Length,
                stackTrace = continueWithStack
            }
        };
    }

    #region async/await chain

    private static async Task Level1Async()
    {
        await Task.Yield();
        await Level2Async();
    }

    private static async Task Level2Async()
    {
        await Task.Yield();
        await Level3Async();
    }

    private static async Task Level3Async()
    {
        await Task.Yield();
        await Level4Async();
    }

    private static async Task Level4Async()
    {
        await Task.Yield();
        throw new InvalidOperationException(
            "This exception is intentional — observe the stack trace depth.");
    }

    #endregion

    #region ContinueWith chain (for comparison)

    private static Task ContinueWithChainAsync()
    {
        return Task.CompletedTask
            .ContinueWith(_ => Task.CompletedTask).Unwrap()
            .ContinueWith(_ => Task.CompletedTask).Unwrap()
            .ContinueWith(_ => Task.CompletedTask).Unwrap()
            .ContinueWith<Task>(_ => throw new InvalidOperationException(
                "ContinueWith exception — this stack is always noisy.")).Unwrap();
    }

    #endregion
}
