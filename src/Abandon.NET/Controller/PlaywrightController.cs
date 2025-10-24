using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;

namespace Abandon.NET.Services;

[ApiController]
[Route("api/[controller]")]
public class PlaywrightController : ControllerBase
{
    [HttpGet("Test")]
    [AllowAnonymous]
    public async Task<string> Test()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        //new playwright
        using var playwright = await Playwright.CreateAsync();

        //new browser
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            Args = ["--start-maximized"] // 启动时最大化
        });

        //new browser context
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport // 使用浏览器实际窗口大小
        });

        //new page
        var homePage = await context.NewPageAsync();
        await homePage.GotoAsync("http://42.200.94.149:8085/cmt/");

        //set localStorage
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNzYxMjczODg3IiwiZXhwIjoxNzYxMjkxODg3LCJzY29wZSI6IldlYkFQSVNlcnZpY2UiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGltb3RoeS5sdSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJDTVRfU3VwZXJBZG1pbiIsIkNNVF9CUF9BZG1pblVzZXIiLCJDTVRfUmVzZWFyY2hlcl9HZW5lcmFsVXNlciJdLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjU3MjYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjU3MjYifQ.GBGuujaZCe3LIao7KDelIOfiHzNkwqX0nOhr2ihT_DI";
        await homePage.EvaluateAsync($"localStorage.setItem('userToken', '{token}')");
        await homePage.EvaluateAsync($"localStorage.setItem('language', 'en-US')");
        await homePage.EvaluateAsync($"localStorage.setItem('USERID', 'timothy.lu')");

        //add header
        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {token}" }
        };
        await homePage.SetExtraHTTPHeadersAsync(headers);

        //goto list page
        await homePage.GotoAsync("http://42.200.94.149:8085/cmt/research/research");
        await homePage.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //找到多个 //span[@class='underline']
        var spans = await homePage.QuerySelectorAllAsync("//span[@class='underline']");

        //测试目的，只处理第一个订单
        //Sit 202500911-D001
        spans = spans.Take(2).ToList();

        //多个订单任务同时处理
        var orderTasks = new Dictionary<string, Task>();
        foreach (var span in spans)
        {
            //点击订单号，打开新标签页
            var newPageTask = context.WaitForPageAsync();
            await span.ClickAsync();
            var detailPage = await newPageTask;

            var reportId = await span.InnerTextAsync();
            var task = ProcessOrder(reportId, detailPage);
            orderTasks.Add(reportId, task);
        }

        //运行前，查询这一批点击的订单的Noresult信息

        //等待所有任务完成
        await Task.WhenAll(orderTasks.Select(t => t.Value));

        //运行后，查询这一批点击的订单的Noresult信息

        //获取所有标签页
        //var pages = context.Pages;

        stopwatch.Stop();
        Log($"所有订单处理完成，耗时：{stopwatch.ElapsedMilliseconds}毫秒");

        //最终返回
        return stopwatch.ElapsedMilliseconds.ToString();
    }

    [NonAction]
    public async Task ProcessOrder(string reportId, IPage detailPage)
    {
        try
        {
            Log($"{reportId}:开始处理");

            //等待页面加载完成
            await detailPage.WaitForLoadStateAsync(LoadState.NetworkIdle);

            Log($"{reportId}:页面加载完成,{detailPage.Url}");
            var url = Base64Decode(ExtractDetailParams(detailPage.Url));
            Log($"{reportId}:解析出的Url参数：{url}");

            //找到NoResult元素，点击勾选
            Log($"{reportId}:查找民事诉讼的NoResult元素");
            var moduleId = 41;
            var noResultCheckbox = await detailPage.QuerySelectorAsync($"//input[@type='checkbox' and @v-module='M{moduleId}' and @v-bind='NoResults']");
            if (noResultCheckbox == null)
            {
                Log($"{reportId}:查找民事诉讼的NoResult元素，未找到");
                await detailPage.CloseAsync();
                return;
            }

            Log($"{reportId}:查找民事诉讼的NoResult元素，找到了，点击");

            //如果已经勾选了，就不点击了
            if (await noResultCheckbox!.IsCheckedAsync())
            {
                Log($"{reportId}:查找民事诉讼的NoResult元素，已经勾选了，点击取消勾选");
                await noResultCheckbox!.UncheckAsync();
            }
            else
            {
                Log($"{reportId}:查找民事诉讼的NoResult元素，未勾选，点击勾选");
                await noResultCheckbox!.CheckAsync();
            }

            //找到保存按钮，点击保存
            Log($"{reportId}:查找保存按钮");
            var saveButton = await detailPage.QuerySelectorAsync(@"//button[@class=""el-tooltip__trigger""]");
            if (saveButton == null)
            {
                Log($"{reportId}:查找保存按钮，未找到，关闭当前标签页");
                await detailPage.CloseAsync();
                return;
            }

            //等待保存请求完成
            Log($"{reportId}:查找保存按钮，找到了，点击保存");
            var responseTask = detailPage.WaitForResponseAsync(response =>
                response.Url.Contains("/ordermangementapi/Orders/InputSave") && response.Status == 200);
            await saveButton.ClickAsync();
            await responseTask;
            Log($"{reportId}:保存请求完成");

            //等待保存请求完成
            //await detailPage.WaitForLoadStateAsync(LoadState.NetworkIdle);

            //关闭当前标签页
            await detailPage.CloseAsync();
        }
        catch (Exception ex)
        {
            Log($"{reportId}:处理过程中发生错误：{ex.Message}");
        }
    }

    private void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
    }

    /// <summary>
    /// 从 URL 中提取 detailParams 参数值
    /// </summary>
    static string ExtractDetailParams(string url)
    {
        // 解析 URL
        Uri uri;
        if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
        {
            return null;
        }

        // 获取查询参数集合
        NameValueCollection queryParams = HttpUtility.ParseQueryString(uri.Query);

        // 返回 detailParams 的值
        return queryParams["detailParams"];
    }

    /// <summary>
    /// Base64 解码（处理 URL 安全的 Base64 字符）
    /// </summary>
    static string Base64Decode(string base64String)
    {
        // 替换 URL 安全的 Base64 字符（如果存在）
        base64String = base64String.Replace('-', '+').Replace('_', '/');

        // 补充 Base64 填充符（=）
        switch (base64String.Length % 4)
        {
            case 2: base64String += "=="; break;
            case 3: base64String += "="; break;
        }

        // 解码
        byte[] bytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(bytes);
    }
}