using System.ComponentModel.DataAnnotations;


namespace KSN_SAS.Models.Entities
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public int Rating { get; set; } 
    }

}
