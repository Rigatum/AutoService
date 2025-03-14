using AutoService.Models;
using Dapper;
using System.Data;
using AutoService.Interfaces;
using Microsoft.Data.SqlClient;

namespace AutoService.Repository;

public class RepairRepository(string? connectionString) : IRepairRepository
{
    public async Task<List<RepairData>> GetNotCompletedRepairsForMonthAsync(int month, int year)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        var parameters = new { Month = month, Year = year };
        var result = await db.QueryAsync<RepairData>(
            "usp_GetNotCompletedRepairsForMonth",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result.AsList();
    }
}