using AutoService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Controllers;

public class RepairsController(IRepairService repairService, ILogger<RepairsController> logger)
    : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ExportToExcel(int month, int year)
    {
        var repairs = await repairService. GetNotCompletedMonthForMonthAsync(month, year);
        var fileContent = repairService.ExportRepairsToExcelAsync(repairs, month, year);

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Repairs.xlsx");
    }
}
