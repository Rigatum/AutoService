using AutoService.Interfaces;
using AutoService.Models;
using ClosedXML.Excel;
using System.Globalization;

namespace AutoService.Services;

public class RepairService(IRepairRepository repairRepository) : IRepairService
{
    public async Task<List<RepairData>> GetNotCompletedMonthForMonthAsync(int month, int year) =>
        await repairRepository.GetNotCompletedRepairsForMonthAsync(month, year);

    public byte[] ExportRepairsToExcelAsync(List<RepairData> repairs, int month, int year) =>
        ExportRepairsToExcel(repairs, month, year);

    private byte[] ExportRepairsToExcel(List<RepairData> repairs, int month, int year)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Repairs");

        var monthYearText = $"{GetMonthName(month)} {year}";
        worksheet.Cell(1, 1).Value = monthYearText;
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;
        worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("A1:I1").Merge();

        var headerStyle = workbook.Style;
        headerStyle.Font.Bold = true;
        headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerStyle.Fill.BackgroundColor = XLColor.LightGray;
        headerStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;

        worksheet.Cell(2, 1).Value = "ФИО Мастера";
        worksheet.Cell(2, 2).Value = "Марка машины";
        worksheet.Cell(2, 3).Value = "Модель машины";
        worksheet.Cell(2, 4).Value = "Дата начала ремонта";
        worksheet.Cell(2, 5).Value = "Дата окончания ремонта";
        worksheet.Cell(2, 6).Value = "Цена ремонта";
        worksheet.Cell(2, 7).Value = "Сумма стоимости ремонтов у мастера за месяц";
        worksheet.Cell(2, 8).Value = "Сумма стоимости ремонтов у всех мастеров за месяц";
        worksheet.Cell(2, 9).Value = "Загруженность мастера за месяц (%)";

        var headerRange = worksheet.Range("A2:I2");
        headerRange.Style = headerStyle;

        for (int i = 0; i < repairs.Count; i++)
        {
            worksheet.Cell(i + 3, 1).Value = repairs[i].MasterFullName;
            worksheet.Cell(i + 3, 2).Value = repairs[i].CarBrand;
            worksheet.Cell(i + 3, 3).Value = repairs[i].CarModel;
            worksheet.Cell(i + 3, 4).Value = repairs[i].StartDate.ToString("dd.MM.yyyy");
            worksheet.Cell(i + 3, 5).Value = repairs[i].EndDate?.ToString("dd.MM.yyyy") ?? "В процессе ремонта";
            worksheet.Cell(i + 3, 6).Value = repairs[i].RepairCost;
            worksheet.Cell(i + 3, 7).Value = repairs[i].MasterTotalCost;
            worksheet.Cell(i + 3, 8).Value = repairs[i].OverallTotalCost;
            worksheet.Cell(i + 3, 9).Value = repairs[i].WorkloadPercentage;
        }

        var dataRange = worksheet.Range($"A3:I{repairs.Count + 2}");
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private string GetMonthName(int month) => CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(month);
}