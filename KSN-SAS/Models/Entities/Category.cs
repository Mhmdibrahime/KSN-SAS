using System.ComponentModel.DataAnnotations;


namespace KSN_SAS.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
