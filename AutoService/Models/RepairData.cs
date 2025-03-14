namespace AutoService.Models;
public class RepairData
{
    public required string MasterFullName { get; set; }
    public required string CarBrand { get; set; }
    public required string CarModel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal RepairCost { get; set; }
    public decimal MasterTotalCost { get; set; }
    public decimal OverallTotalCost { get; set; }
    public decimal WorkloadPercentage { get; set; }
}
