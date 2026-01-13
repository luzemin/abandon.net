using Abandon.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abandon.NET.Services;

[ApiController]
[Route("api/[controller]")]
public class ExcelTestController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Test()
    {
        var fileName = $"Test_{DateTime.Now:yyyyMMdd HHmmss}.xlsx";
        var bytes = MyEmailExcelService.GenerateExcel(Records);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    private static List<SkyRecord> Records =
    [
        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0002",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Profile", "企业信息"],
            Level = ["Standard", "标准"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0003",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Civil Litigation Search", "民事诉讼记录"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0004",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Civil Litigation Search", "民事诉讼记录"],
            Level = ["Mandatory", "合规"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0005",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Winding-up Search", "清盘调查"],
            Level = ["Mandatory", "合规"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0006",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Irregularity & Disciplinary Action Search", "违规及纪律处分调查"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0007",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Media Record Search", "媒体记录调查"],
            Level = ["Simp. Chi. Name", "简体中文名"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0008",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["China"],
            ProductName = ["Reputation Intelligence", "声誉资讯"],
            Level = ["Standard", "标准"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0009",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["Hong Kong, China"],
            ProductName = ["Civil Litigation Search", "民事诉讼记录"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0010",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["Hong Kong, China"],
            ProductName = ["Criminal Record Search", "刑事记录调查"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0011",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd", "苏州易慕峰生物科技有限公司", "苏州易慕峰生物科技有限公司"],
            Country = ["Hong Kong, China"],
            ProductName = ["Winding-up Search", "清盘调查"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0012",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd2", "苏州易慕峰生物科技有限公司2", "苏州易慕峰生物科技有限公司2"],
            Country = ["Hong Kong, China"],
            ProductName = ["Irregularity & Disciplinary Action Search", "违规及纪律处分调查"],
            Level = [""]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0013",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd2", "苏州易慕峰生物科技有限公司2", "苏州易慕峰生物科技有限公司2"],
            Country = ["Hong Kong, China"],
            ProductName = ["Media Record Search", "媒体记录调查"],
            Level = ["Trad. Chi. Name", "繁体中文名"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0014",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd2", "苏州易慕峰生物科技有限公司2", "苏州易慕峰生物科技有限公司2"],
            Country = ["Hong Kong, China"],
            ProductName = ["Media Record Search", "媒体记录调查"],
            Level = ["Eng Name", "英文名"]
        },

        new SkyRecord
        {
            SearchItemCode = "PCN251200033-0015",
            SkySubjectName = ["Suzhou Yimufeng Biotechnology Co ltd3", "苏州易慕峰生物科技有限公司3", "苏州易慕峰生物科技有限公司3"],
            Country = ["Worldwide"],
            ProductName = ["Global Compliance Check", "全球合规调查"],
            Level = [""]
        }
    ];
}