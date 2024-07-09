using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.ViewModels.Cloud;
using Microsoft.AspNetCore.Authorization;

namespace ZodiacJewelryWebApI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly ImageService _imageService;
        private readonly AppDbContext _context;

        public ProductImagesController(IOptions<CloudinarySettings> config, AppDbContext context,
            ImageService imageService)
        {
            var cloudinaryAccount = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(cloudinaryAccount);
            _context = context;
            _imageService = imageService;
        }

        [Authorize(Roles = "Staff,Admin")]
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> UploadProductImages(int productId, [FromForm] List<IFormFile> files)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            var uploadedImageUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Crop("fill").Gravity("face")
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.Url != null)
                        {
                            uploadedImageUrls.Add(uploadResult.Url.ToString());

                            var productImage = new ProductImage
                            {
                                ProductId = productId,
                                ImageUrl = uploadResult.Url.ToString()
                            };

                            _context.ProductImage.Add(productImage);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { imageUrls = uploadedImageUrls });
        }

        [Authorize(Roles = "Staff,Admin")]
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateProductImage(int productId, int imageId, IFormFile file)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            var productImage =
                await _context.ProductImage.FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == imageId);
            if (productImage == null)
                return NotFound("Product image not found");

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            if (uploadResult.Url == null)
                return BadRequest("Could not upload image");

            // Update the image URL in the database
            productImage.ImageUrl = uploadResult.Url.ToString();
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = productImage.ImageUrl });
        }

        [Authorize(Roles = "Staff,Admin")]
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveProductImage(int productId, int imageId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            var productImage =
                await _context.ProductImage.FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == imageId);
            if (productImage == null)
                return NotFound("Product image not found");

            {
                if (productImage.ImageUrl != null)
                {
                    var publicId =
                        _imageService.GetPublicIdFromImageUrl(productImage.ImageUrl);

                    var deletionParams = new DeletionParams(publicId);

                    var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                    if (deletionResult.Result == "deleted") return Ok("Product image deleted successfully");
                    _context.ProductImage.Remove(productImage);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("Product image deleted successfully");
        }
    }
}