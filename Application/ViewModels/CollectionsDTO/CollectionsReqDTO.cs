using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.CollectionsDTO;

public class CollectionsReqDTO
{
    public string NameCollection { get; set; }
    public IFormFile ImageCollection { get; set; }
    public DateTime DateClose { get; set; }
}

public class CollectionStatusReqDTO
{
    public byte Status { get; set; }
}