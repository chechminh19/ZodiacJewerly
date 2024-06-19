namespace Application.ViewModels.CollectionsDTO;

public class CollectionsResDTO
{
    public int Id { get; set; }
    public string NameCollection { get; set; }
    public string ImageCollection { get; set; }
    public DateTime DateOpen { get; set; }
    public DateTime DateClose { get; set; }
    public byte Status { get; set; }
}
