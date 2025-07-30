namespace KSN_SAS.Models.DTOs.CategoryAdminDTOs
{
    public class CreateCategoryDto
    {
       public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }

    }
}
