using System.ComponentModel.DataAnnotations;


namespace KSN_SAS.Models.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public bool IsPrimary { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }

}
