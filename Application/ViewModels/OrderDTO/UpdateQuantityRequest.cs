namespace Application.ViewModels.OrderDTO;

public class UpdateQuantityRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}