using KSN_SAS.Models.DTOs.ProductAdminDTOs;

namespace KSN_SAS.Models.DTOs.CategoryAdminDTOs
{
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<ProductsCategory> Products { get; set; }
    }
}
