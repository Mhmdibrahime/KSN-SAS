using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KSN_SAS.Models.Entities;
using KSN_SAS.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KSN_SAS.Models.DTOs.CategoryUserDTOs;
using KSN_SAS.Models.Data;

namespace KSN_SAS.Controllers.User
{
    [Route("User/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(AppDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

       
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Products) 
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        ProductCount = c.Products.Count
                    })
                    .ToListAsync();

                if (!categories.Any())
                {
                    return NotFound("No categories found");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, "An error occurred while retrieving categories");
            }
        }

     
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .Where(c => c.Id == id)
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        ProductCount = c.Products.Count
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {id}");
                return StatusCode(500, $"An error occurred while retrieving category with ID {id}");
            }
        }
    }
}