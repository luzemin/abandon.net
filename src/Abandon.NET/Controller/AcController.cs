using Ganss.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abandon.NET.Services;

[ApiController]
[Route("api/[controller]")]
public class AcController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<dynamic> Test()
    {
        var ac = new AhoCorasick("C#", "Java", "微软", "谷歌");
        var text = @"C#是一种由微软开发的多范式编程语言，它结合了C语言的强大功能和C++的灵活性。C#非常适合开发Windows应用程序。";
        var results = ac.Search(text).ToList();
        return results;
    }
}