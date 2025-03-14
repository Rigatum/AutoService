using AutoService.Models;

namespace AutoService.Interfaces;
public interface IRepairRepository
{
    Task<List<RepairData>> GetNotCompletedRepairsForMonthAsync(int month, int year);
}
