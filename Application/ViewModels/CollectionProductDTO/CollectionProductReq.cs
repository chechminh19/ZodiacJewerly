using Domain.Entities;

namespace Application.ViewModels.CollectionProductDTO;

public class CollectionProductReq
{
    public Product Product { get; set; } = null!;
    public Collections Colections { get; set; } = null!;
}