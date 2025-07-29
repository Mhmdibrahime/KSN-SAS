using System.ComponentModel.DataAnnotations;


namespace KSN_SAS.Models.Entities
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10000)]
        public string? Comment { get; set; }


        public int Rating { get; set; } 
        public DateTime Date { get; set; } 
        public string Status { get; set; } 
        public string UserName { get; set; } 
        public string UserJob { get; set; } 

        
    }

}
