namespace KSN_SAS.Models.DTOs.CategoryAdminDTOs
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
