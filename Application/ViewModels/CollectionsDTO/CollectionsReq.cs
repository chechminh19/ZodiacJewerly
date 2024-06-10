namespace Application.ViewModels.CollectionsDTO;

public class CollectionsReq
{
    public string NameCollection { get; set; }
    public string ImageCollection { get; set; }
    public DateTime DateOpen { get; set; }
    public DateTime DateClose { get; set; }
}