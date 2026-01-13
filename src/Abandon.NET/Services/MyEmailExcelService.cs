using Abandon.NET.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Abandon.NET.Services;

public class MyEmailExcelService
{
    public static byte[] GenerateExcel(List<SkyRecord> records)
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Sky Push Success Records");

        // Create header style
        var headerStyle = CreateHeaderStyle(workbook);
        var cellStyle = CreateCellStyle(workbook);

        // Create header row
        var headerRow = sheet.CreateRow(0);
        var headers = new[] { "Search Item Code", "Subject Name", "Country", "Product", "Level" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = headerRow.CreateCell(i);
            cell.SetCellValue(headers[i]);
            cell.CellStyle = headerStyle;
        }

        // Sort by SearchItemCode
        var sortedRecords = records
            .OrderBy(r => r.SearchItemCode)
            .ToList();

        int currentRow = 1;

        // Process all records and fill data
        for (int i = 0; i < sortedRecords.Count; i++)
        {
            var record = sortedRecords[i];
            var row = sheet.CreateRow(currentRow);

            // Search Item Code
            var codeCell = row.CreateCell(0);
            codeCell.SetCellValue(record.SearchItemCode);
            codeCell.CellStyle = cellStyle;

            // Subject Name
            var subjectCell = row.CreateCell(1);
            subjectCell.SetCellValue(string.Join("\n", record.SkySubjectName));
            subjectCell.CellStyle = cellStyle;

            // Country
            var countryCell = row.CreateCell(2);
            countryCell.SetCellValue(string.Join("\n", record.Country));
            countryCell.CellStyle = cellStyle;

            // Product Name
            var productCell = row.CreateCell(3);
            productCell.SetCellValue(string.Join("\n", record.ProductName));
            productCell.CellStyle = cellStyle;

            // Level
            var levelCell = row.CreateCell(4);
            levelCell.SetCellValue(string.Join("\n", record.Level));
            levelCell.CellStyle = cellStyle;

            currentRow++;
        }

        int totalRows = sortedRecords.Count;

        // Store merge boundaries for each column (column index -> list of merge regions)
        var columnMergeBoundaries = new Dictionary<int, List<(int start, int end)>>();

        // Process each column from left to right (starting from column 1 - Subject Name)
        for (int col = 1; col < headers.Length; col++)
        {
            var mergeRegions = new List<(int start, int end)>();

            // Get previous column's merge boundaries (if exists)
            List<(int start, int end)> previousBoundaries = null;
            if (col > 1 && columnMergeBoundaries.ContainsKey(col - 1))
            {
                previousBoundaries = columnMergeBoundaries[col - 1];
            }

            int mergeStart = 1;
            string currentValue = GetCellValue(sortedRecords[0], col);

            for (int i = 1; i <= totalRows; i++)
            {
                bool shouldEndMerge = false;
                string nextValue = null;

                if (i < totalRows)
                {
                    nextValue = GetCellValue(sortedRecords[i], col);

                    // Check if value changes
                    if (nextValue != currentValue)
                    {
                        shouldEndMerge = true;
                    }

                    // Check if we're crossing a previous column's merge boundary
                    if (previousBoundaries != null && !shouldEndMerge)
                    {
                        // Check if current row (i) and next row (i+1) are in different merge regions of previous column
                        var currentRegion = previousBoundaries.FirstOrDefault(b => b.start <= i && i <= b.end);
                        var nextRegion = previousBoundaries.FirstOrDefault(b => b.start <= i + 1 && i + 1 <= b.end);

                        if (currentRegion != nextRegion)
                        {
                            shouldEndMerge = true;
                        }
                    }
                }
                else
                {
                    shouldEndMerge = true;
                }

                if (shouldEndMerge)
                {
                    // Record this merge region
                    mergeRegions.Add((mergeStart, i));

                    // Add merge to sheet if more than 1 row
                    if (i > mergeStart)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(mergeStart, i, col, col));
                    }

                    if (i < totalRows)
                    {
                        mergeStart = i + 1;
                        currentValue = nextValue;
                    }
                }
            }

            // Store merge boundaries for this column
            columnMergeBoundaries[col] = mergeRegions;
        }

        // Auto-size columns
        for (int i = 0; i < headers.Length; i++)
        {
            sheet.AutoSizeColumn(i);
            // Add some extra width for better readability
            sheet.SetColumnWidth(i, sheet.GetColumnWidth(i) + 2000);
        }

        // Convert to byte array
        using (var memoryStream = new MemoryStream())
        {
            workbook.Write(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private static string GetCellValue(SkyRecord record, int columnIndex)
    {
        return columnIndex switch
        {
            1 => string.Join("|", record.SkySubjectName),
            2 => record.Country[0],
            3 => string.Join("|", record.ProductName),
            4 => string.Join("|", record.Level),
            _ => ""
        };
    }

    private static ICellStyle CreateHeaderStyle(IWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        font.FontHeightInPoints = 11;
        style.SetFont(font);
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        return style;
    }

    private static ICellStyle CreateCellStyle(IWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        style.WrapText = true;
        return style;
    }
}