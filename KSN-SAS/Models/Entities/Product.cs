using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KSN_SAS.Models.Entities
{


    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Discount { get; set; }

        [Required, MaxLength(50)]
        public string? Status { get; set; } 

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();
    }

}
