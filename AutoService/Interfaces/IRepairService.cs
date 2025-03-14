using AutoService.Models;

namespace AutoService.Interfaces;

public interface IRepairService
{
    Task<List<RepairData>> GetNotCompletedMonthForMonthAsync(int month, int year);
    byte[] ExportRepairsToExcelAsync(List<RepairData> repairs, int month, int year);
}