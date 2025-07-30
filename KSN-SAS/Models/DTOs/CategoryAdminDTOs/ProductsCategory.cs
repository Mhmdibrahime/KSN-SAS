using KSN_SAS.Models.DTOs.ProductAdminDTOs;

namespace KSN_SAS.Models.DTOs.CategoryAdminDTOs
{
    public class ProductsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }

        
        public ProductImage? PrimaryImage { get; set; }
    }
}
