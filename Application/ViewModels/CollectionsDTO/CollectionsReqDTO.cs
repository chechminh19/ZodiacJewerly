using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.CollectionsDTO;

public class CollectionsReqDTO
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
    public string? NameCollection { get; set; }
    public IFormFile? ImageCollection { get; set; } 
    public DateTime DateClose { get; set; }
}

public class CollectionStatusReqDTO
{
    public byte Status { get; set; }
}