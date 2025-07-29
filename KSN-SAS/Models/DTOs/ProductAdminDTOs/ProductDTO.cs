﻿namespace KSN_SAS.Models.DTOs.ProductAdminDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}