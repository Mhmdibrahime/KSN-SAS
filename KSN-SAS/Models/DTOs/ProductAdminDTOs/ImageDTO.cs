namespace KSN_SAS.Models.DTOs.ProductAdminDTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
        public bool ToDelete { get; set; }
    }
}