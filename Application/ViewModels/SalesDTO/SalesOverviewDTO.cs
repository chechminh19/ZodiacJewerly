using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.SalesDTO;

public class SalesOverviewDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double TotalSales { get; set; }
    public int TotalOrders { get; set; }
}