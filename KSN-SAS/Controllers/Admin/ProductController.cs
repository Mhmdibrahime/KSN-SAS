using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KSN_SAS.Models.Entities;
using KSN_SAS.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using KSN_SAS.Models.DTOs.ProductAdminDTOs;
using KSN_SAS.Models.Data;

namespace KSN_SAS.Controllers.Admin
{
    [Route("Admin/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Discount = p.Discount,
                    Status = p.Status,
                    CategoryId = p.CategoryId,
                    Images = p.Images.Select(i => new ImageDTO
                    {
                        Id = i.Id,
                        Url = i.Url,
                        IsPrimary = i.IsPrimary
                    }).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Discount = product.Discount,
                Status = product.Status,
                CategoryId = product.CategoryId,
                Images = product.Images.Select(i => new ImageDTO
                {
                    Id= i.Id,
                    Url = i.Url,
                    IsPrimary = i.IsPrimary
                }).ToList()
            };

            return Ok(productDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == model.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId");
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                Discount = model.Discount,
                Status = model.Status,
                CategoryId = model.CategoryId,
                Images = new List<Image>()
            };

            if (model.NewImages != null && model.NewImages.Count > 0)
            {
                foreach (var image in model.NewImages)
                {
                    if (image.Length > 0)
                    {
                        var imagePath = await UploadImage(image);
                        product.Images.Add(new Image
                        {
                            Url = imagePath,
                            IsPrimary = product.Images.Count == 0 
                        });
                    }
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var createdProductDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Discount = product.Discount,
                Status = product.Status,
                CategoryId = product.CategoryId,
                Images = product.Images.Select(i => new ImageDTO
                {
                    Id = i.Id,
                    Url = i.Url,
                    IsPrimary = i.IsPrimary
                }).ToList()
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createdProductDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest("ID mismatch");
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == model.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId");
            }

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.Discount = model.Discount;
            product.Status = model.Status;
            product.CategoryId = model.CategoryId;

            if (model.ImagesToDelete != null && model.ImagesToDelete.Any())
            {
                foreach (var imageId in model.ImagesToDelete)
                {
                    var imageToDelete = product.Images.FirstOrDefault(i => i.Id == imageId);
                    if (imageToDelete != null)
                    {
                        DeleteImage(imageToDelete.Url);
                        _context.Images.Remove(imageToDelete);
                    }
                }
            }

            if (model.NewImages != null && model.NewImages.Any())
            {
                foreach (var image in model.NewImages)
                {
                    if (image.Length > 0)
                    {
                        var imagePath = await UploadImage(image);
                        product.Images.Add(new Image
                        {
                            Url = imagePath,
                            IsPrimary = !product.Images.Any(i => i.IsPrimary) 
                        });
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private void DeleteImage(string imageUrl)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            foreach (var image in product.Images)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.Url.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Images.RemoveRange(product.Images);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("No image file provided");
            }

            if (string.IsNullOrWhiteSpace(_webHostEnvironment.WebRootPath))
            {
                _webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/products/{uniqueFileName}";
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}