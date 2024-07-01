using Domain.Entities;

namespace Application.ViewModels.CollectionsDTO;

public class CollectionsResDTO
{
    public int Id { get; set; }
    public string? NameCollection { get; set; }
    public string? ImageCollection { get; set; }
    public DateTime DateOpen { get; set; }
    public DateTime DateClose { get; set; }
    public byte Status { get; set; }
}

public class CollectionDetailResDTO
{
    public int Id { get; set; }
    public string NameCollection { get; set; }
    public string ImageCollection { get; set; }
    public DateTime DateOpen { get; set; }
    public DateTime DateClose { get; set; }
    public List<ProductOfCollectionResDTO> Products { get; set; }
}

public class ProductOfCollectionResDTO
{
    public int Id { get; set; }
    public string NameProduct { get; set; }
    public string DescriptionProduct { get; set; }       
    public double Price { get; set; }
    public List<ProductImageCollectionDTO> ImageUrls { get; set; }
}

public class ProductImageCollectionDTO
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
}
