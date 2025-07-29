using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KSN_SAS.Models.DTOs.ProductAdminDTOs
{

    public class CreateProductDTO
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        public int CategoryId { get; set; }

        public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();
    }
}