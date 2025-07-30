using KSN_SAS.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KSN_SAS.Models.DTOs;
using KSN_SAS.Models.DTOs.CategoryAdminDTOs;
using KSN_SAS.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using KSN_SAS.Models.DTOs.ProductAdminDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



namespace KSN_SAS.Controllers.Admin
{
    [Authorize]
    [Route("Admin/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("Categories")]
        public IActionResult GetAllCategry()
        {
            var Categories = context.Categories
            .Select(x=>new ShowCategories
            {
                Id=x.Id,
                Name=x.Name
            }
            ).ToList();

            return Ok(Categories);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategry([FromForm] CreateCategoryDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("Image file is required.");

            
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string imageFolder = Path.Combine(wwwRootPath, "images", "categories");

          
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

          
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
            string fullPath = Path.Combine(imageFolder, uniqueFileName);

            
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }

            
            var category = new Category
            {
                Name = dto.Name,
                ImageUrl = $"/images/categories/{uniqueFileName}"
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return Ok(new { message = "Category created successfully", categoryId = category.Id });
        }
        [HttpGet("{id}")]
        public IActionResult GetCategoryWithProducts(int id)
        {
            var category = context.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Images)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            var result = new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                Products = category.Products.Select(p => new ProductsCategory
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Discount = p.Discount,
                    Status = p.Status,
                    CategoryId = p.CategoryId,
                    PrimaryImage = p.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => new ProductImage
                        {
                           
                            Url = i.Url
                        })
                        .FirstOrDefault()
                }).ToList()
            };

            return Ok(result);
        }
        [HttpDelete("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            context.Categories.Remove(category);
            context.SaveChanges();

            return Ok(new { message = "Category deleted successfully" });
        }
        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id,[FromForm] UpdateCategoryDto dto)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == Id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            category.Name = dto.Name;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string imageFolder = Path.Combine(wwwRootPath, "images", "categories");

                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
                string fullPath = Path.Combine(imageFolder, uniqueFileName);

                // Save new image
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    string oldImagePath = Path.Combine(wwwRootPath, category.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                category.ImageUrl = $"/images/categories/{uniqueFileName}";
            }

            await context.SaveChangesAsync();
            return Ok(new { message = "Category updated successfully", categoryId = category.Id });
        }

    }
}
